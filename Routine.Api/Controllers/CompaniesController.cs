﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.Api.Models;
using Routine.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Routine.Api.DtoParameters;
using Routine.Api.Entities;

namespace Routine.Api.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompaniesController(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository ??
                throw new ArgumentNullException(nameof(companyRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper)); ;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies(
           [FromQuery] CompanyDtoParameters parameters)
        {
            var companies = await _companyRepository.GetCompaniesAsync(parameters);

            var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            return Ok(companyDtos);
        }

        [HttpGet("{companyId}", Name = nameof(GetCompany))] //   api/companies/{companyId}
        public async Task<ActionResult<CompanyDto>> GetCompany(Guid companyId)
        {
            var company = await _companyRepository.GetCompanyAsync(companyId);
            if (company == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CompanyDto>(company));
        }

        [HttpPost]
        public async Task<ActionResult<CompanyDto>> CreateCompany(CompanyAddDto company)
        {
            var entity = _mapper.Map<Company>(company);
            _companyRepository.AddCompany(entity);
            await _companyRepository.SaveAsync();

            var returnDto = _mapper.Map<CompanyDto>(entity);

            return CreatedAtRoute(nameof(GetCompany), new { companyId = returnDto.Id }, returnDto);
        }
    }
}