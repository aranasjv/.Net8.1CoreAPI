using AutoMapper;
using Microsoft.Extensions.Configuration;
using TestAPI.DTOs;
using TestAPI.Entities;
using TestAPI.Interfaces.Repositories;
using TestAPI.Interfaces.Services;
using TestAPI.Repositories;

namespace TestAPI.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _repository;
        private readonly MongoDepartmentRepository _mongoRepository;
        // private readonly PostgresDepartmentRepository _postgresRepository; // Uncomment if implemented
        private readonly IMapper _mapper;
        private readonly int _dataSource;

        public DepartmentService(
            IDepartmentRepository repository,
            MongoDepartmentRepository mongoRepository,
            // PostgresDepartmentRepository postgresRepository, // Uncomment if implemented
            IMapper mapper,
            IConfiguration configuration)
        {
            _repository = repository;
            _mongoRepository = mongoRepository;
            // _postgresRepository = postgresRepository; // Uncomment if implemented
            _mapper = mapper;
            _dataSource = configuration.GetValue<int>("DepartmentDataSource");
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
        {
            // Fetches from the configured data source (EF, MongoDB, or PostgreSQL)
            var departments = _dataSource switch
            {
                2 => await _mongoRepository.GetAllAsync(), // MongoDB
                // 3 => await _postgresRepository.GetAllAsync(), // PostgreSQL (if implemented)
                _ => await _repository.GetAllAsync(), // EF
            };
            return _mapper.Map<IEnumerable<DepartmentDto>>(departments);
        }

        public async Task<DepartmentDto?> GetByIdAsync(int departmentId)
        {
            // Fetches a single department from the configured data source
            Department? department = _dataSource switch
            {
                2 => await _mongoRepository.GetByIdAsync(departmentId), // MongoDB
                // 3 => await _postgresRepository.GetByIdAsync(departmentId), // PostgreSQL (if implemented)
                _ => await _repository.GetByIdAsync(departmentId), // EF
            };
            return department == null ? null : _mapper.Map<DepartmentDto>(department);
        }

        public async Task<DepartmentDto> AddAsync(DepartmentDto dto)
        {
            var department = _mapper.Map<Department>(dto);
            var departmentMongoDB = _mapper.Map<DepartmenMongoDB>(dto);

            // Always check existence in both EF and MongoDB before insert
            bool existsInEf = await _repository.ExistsAsync(department.DepartmentName);
            bool existsInMongo = await _mongoRepository.ExistsAsync(department.DepartmentName);
            if (existsInEf || existsInMongo)
                throw new InvalidOperationException("Department Name Already Exists in EF or MongoDB.");

            // Get max DepartmentId from both sources
            int maxEfId = 0;
            int maxMongoId = 0;

            // Entity Framework Insertion and checking if available
            var efDepartments = await _repository.GetAllAsync();
            if (efDepartments.Any())
                maxEfId = efDepartments.Max(d => d.DepartmentId);

            await _repository.AddAsync(department);

            //MongoDB Insertion and checking if available
            var mongoDepartments = await _mongoRepository.GetAllAsync();
            if (mongoDepartments.Any())
                maxMongoId = mongoDepartments.Max(d => d.DepartmentId);

            // Calculate the next available DepartmentId for mongoDB
            int nextId = Math.Max(maxEfId, maxMongoId) + 1;

            // If DepartmentId is not set or is 0, assign the nextId
            if (departmentMongoDB.DepartmentId == 0)
                departmentMongoDB.DepartmentId = nextId;

            // Insert into MongoDB (ensure Id is null so MongoDB generates a new _id)
            departmentMongoDB.Id = null;
            await _mongoRepository.AddAsync(departmentMongoDB);

            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task<bool> UpdateAsync(int departmentId, DepartmentDto dto)
        {
            var department = await _repository.GetByIdAsync(departmentId);
            var departmentMongoDB = await _mongoRepository.GetByIdAsync(departmentId);
            if (department == null || departmentMongoDB == null)
                return false;

            _mapper.Map(dto, department);
            _mapper.Map(dto, departmentMongoDB);
            // Update in EF
            await _repository.UpdateAsync(department);
            // Update in MongoDB
            await _mongoRepository.UpdateAsync(departmentMongoDB);
            return true;
        }

        public async Task<bool> DeleteAsync(int departmentId)
        {
            var department = await _repository.GetByIdAsync(departmentId);
            if (department == null)
                return false;

            // Delete from EF
            await _repository.DeleteAsync(departmentId);
            // Delete from MongoDB
            await _mongoRepository.DeleteAsync(departmentId);
            return true;
        }
        public async Task<bool> IsDepartmentExist(string departmentName)
        {
            // Checks existence in the configured data source
            return await IsDepartmentExistEf(departmentName) && await IsDepartmentExistMongo(departmentName);
        }

        // Checks existence in EF only
        public async Task<bool> IsDepartmentExistEf(string departmentName)
        {
            return await _repository.ExistsAsync(departmentName);
        }

        // Checks existence in MongoDB only
        public async Task<bool> IsDepartmentExistMongo(string departmentName)
        {
            return await _mongoRepository.ExistsAsync(departmentName);
        }

    }
}


