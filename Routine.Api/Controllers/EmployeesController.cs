﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Routine.Api.Entities;
using Routine.Api.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Routine.Api.Controllers
{
    [ApiController]
    [Route("api/companies/{companyId}/employees")]
    public class EmployeesController: ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;

        public EmployeesController(IMapper mapper, ICompanyRepository companyRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> 
            GetEmployeesFromCompany(Guid companyId, [FromQuery(Name = "gender")]string genderDisplay,
                string q)
        {
            if (! await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var employees = await _companyRepository
                .GetEmployeesAsync(companyId, genderDisplay, q);

            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return Ok(employeeDtos);
        }

        [HttpGet("{employeeId}", Name = nameof(GetEmployeeFromCompany))]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeFromCompany(Guid companyId, Guid employeeId)
        {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var employee = await _companyRepository.GetEmployeeAsync(companyId, employeeId);

            if (employee == null)
            {
                return NotFound();
            }

            
            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            return Ok(employeeDto);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> CreateEmployeeForCompany(Guid companyId, EmployeeAddDto employee)
        {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var entity = _mapper.Map<Employee>(employee);

            _companyRepository.AddEmployee(companyId, entity);
            await _companyRepository.SaveAsync();

            var dtoToReturn = _mapper.Map<EmployeeDto>(entity);

            return CreatedAtRoute(nameof(GetEmployeeFromCompany), new
            {
                companyId = companyId,
                employeeId = dtoToReturn.Id
            }, dtoToReturn);
        }

        [HttpPut(template: "{employeeId}")]
        public async Task<ActionResult<EmployeeDto>> UpdateEmployeeForCompany(
            Guid companyId, Guid employeeId, EmployeeUpdateDto employee)
        {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var employeeEntity = await _companyRepository.GetEmployeeAsync(companyId, employeeId);

            EmployeeDto dtoToReturn;
            if (employeeEntity == null)
            {
                var employeeToAddEntity = _mapper.Map<Employee>(employee);
                employeeToAddEntity.Id = employeeId;

                _companyRepository.AddEmployee(companyId, employeeToAddEntity);

                await _companyRepository.SaveAsync();

                dtoToReturn = _mapper.Map<EmployeeDto>(employeeToAddEntity);

                return CreatedAtRoute(nameof(GetEmployeeFromCompany), new
                {
                    companyId,
                    employeeId = dtoToReturn.Id
                }, dtoToReturn);
            }

            // entity 转化为 updateDto
            // 把传进来的employee 的值更新到  updateDto
            // 把updateDto映射回 entity
            _mapper.Map(employee, employeeEntity);

            _companyRepository.UpdateEmployee(employeeEntity);

            await _companyRepository.SaveAsync();

            dtoToReturn = _mapper.Map<EmployeeDto>(employeeEntity); 

            return Ok(dtoToReturn);
        }

        [HttpPatch("{employeeId}")]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(
            Guid companyId, Guid employeeId, JsonPatchDocument<EmployeeUpdateDto> patchDocument)
        {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var employeeEntity = await _companyRepository.GetEmployeeAsync(companyId, employeeId);

           
            if (employeeEntity == null)
            {
                var employeeDto = new EmployeeUpdateDto();
                patchDocument.ApplyTo(employeeDto, ModelState);

                if (!TryValidateModel(employeeDto))
                {
                    return ValidationProblem(ModelState);
                }

                var employeeToAdd = _mapper.Map<Employee>(employeeDto);
                employeeToAdd.Id = employeeId;

                _companyRepository.AddEmployee(companyId, employeeToAdd);
                await _companyRepository.SaveAsync();

                var dtoToReturn = _mapper.Map<EmployeeDto>(employeeToAdd);

                return CreatedAtRoute(nameof(GetEmployeeFromCompany), new
                {
                    companyId,
                    employeeId = dtoToReturn.Id
                }, dtoToReturn);
            }

            var dtoToPatch = _mapper.Map<EmployeeUpdateDto>(employeeEntity);

            //需要处理验证错误
            patchDocument.ApplyTo(dtoToPatch, ModelState);

            if (!TryValidateModel(dtoToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(dtoToPatch, employeeEntity);

            _companyRepository.UpdateEmployee(employeeEntity);

            await _companyRepository.SaveAsync();

            return NoContent();
        }

        public override ActionResult ValidationProblem(ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices
                .GetRequiredService<IOptions<ApiBehaviorOptions>>();

            return (ActionResult) options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}
