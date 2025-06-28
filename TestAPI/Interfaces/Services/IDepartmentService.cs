﻿using TestAPI.DTOs;

namespace TestAPI.Interfaces.Services
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDto>> GetAllAsync();
        Task<DepartmentDto?> GetByIdAsync(int id);
        Task<DepartmentDto> AddAsync(DepartmentDto dto);
        Task<bool> UpdateAsync(int id, DepartmentDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> IsDepartmentExist(string departmentName);

    }

}
