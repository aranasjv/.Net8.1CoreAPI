using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TestAPI.Entities;

namespace TestAPI.Data;

public partial class AppDbContext : DbContext
{

    private readonly IConfiguration _configuration;

    public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }


    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("Department");

            entity.HasKey(e => e.DepartmentId); 

            entity.Property(e => e.DepartmentId).ValueGeneratedOnAdd();
            entity.Property(e => e.DepartmentName).HasMaxLength(500);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee");

            entity.HasKey(e => e.EmployeeId); 

            entity.Property(e => e.EmployeeId).ValueGeneratedOnAdd();
            entity.Property(e => e.EmployeeName).HasMaxLength(500);
            entity.Property(e => e.Department).HasMaxLength(500);
            entity.Property(e => e.DateOfJoining).HasColumnType("datetime");
            entity.Property(e => e.PhotoFileName).HasMaxLength(500);
        });

        OnModelCreatingPartial(modelBuilder);
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
