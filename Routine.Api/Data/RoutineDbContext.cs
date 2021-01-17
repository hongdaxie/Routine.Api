using Microsoft.EntityFrameworkCore;
using Routine.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Data
{
    public class RoutineDbContext: DbContext
    {
        public RoutineDbContext(DbContextOptions<RoutineDbContext> options): base(options)
        {

        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().Property(x => x.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Company>().Property(x => x.Introduction).HasMaxLength(500);

            modelBuilder.Entity<Employee>().Property(x => x.EmployeeNo).IsRequired().HasMaxLength(10);
            modelBuilder.Entity<Employee>().Property(x => x.FirstName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Employee>().Property(x => x.LastName).IsRequired().HasMaxLength(50);

            modelBuilder.Entity<Employee>()
                .HasOne(x => x.Company)
                .WithMany(x => x.Employees)
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    Id = Guid.Parse("bbdee09c-089b-4d30-bece-44df5923716c"),
                    Name = "Microsoft",
                    Introduction = "Great Company"
                },
                new Company
                {
                    Id = Guid.Parse("ad147e58-4513-4f7f-86de-6972e6a3d9a8"),
                    Name = "Google",
                    Introduction = "Don`t be evil"
                },
                new Company
                {
                    Id = Guid.Parse("72f7850c-f5e8-4762-98d7-e9709cf50750"),
                    Name = "Alipapa",
                    Introduction = "Fubao Company"
                }
            );
        }
    }
}
