using AutoMapper;
using TestAPI.DTOs;
using TestAPI.Entities;
using TestAPI.Interfaces.Repositories;
using TestAPI.Interfaces.Services;

namespace TestAPI.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _repository;
        private readonly IMapper _mapper;

        public DepartmentService(IDepartmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
        {
            var departments = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<DepartmentDto>>(departments);
        }

        public async Task<DepartmentDto?> GetByIdAsync(int departmentId)
        {
            var department = await _repository.GetByIdAsync(departmentId);
            return department == null ? null : _mapper.Map<DepartmentDto>(department);
        }

        public async Task<DepartmentDto> AddAsync(DepartmentDto dto)
        {
            var department = _mapper.Map<Department>(dto);
            await _repository.AddAsync(department);
            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task<bool> UpdateAsync(int departmentId, DepartmentDto dto)
        {
            var department = await _repository.GetByIdAsync(departmentId);
            if (department == null)
                return false;

            _mapper.Map(dto, department);
            await _repository.UpdateAsync(department);
            return true;
        }

        public async Task<bool> DeleteAsync(int departmentId)
        {
            var department = await _repository.GetByIdAsync(departmentId);
            if (department == null)
                return false;

            await _repository.DeleteAsync(departmentId);
            return true;
        }
        public async Task<bool> IsDepartmentExist(string departmentName)
        {
            return await _repository.ExistsAsync(departmentName);
        }

    }

}


