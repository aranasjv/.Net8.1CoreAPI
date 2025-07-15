using MongoDB.Bson;
using MongoDB.Driver;
using TestAPI.Entities;
using TestAPI.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;

namespace TestAPI.Repositories
{
    public class MongoDepartmentRepository : IDepartmentRepository
    {
        private readonly IMongoCollection<Department> _departments;

        public MongoDepartmentRepository(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
            var database = client.GetDatabase(configuration["MongoDbSettings:Database"]);
            _departments = database.GetCollection<Department>("Departments");
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _departments.Find(_ => true).ToListAsync();
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _departments.Find(d => d.DepartmentId == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Department department)
        {
            await _departments.InsertOneAsync(department);
        }

        public async Task UpdateAsync(Department department)
        {
            await _departments.ReplaceOneAsync(d => d.DepartmentId == department.DepartmentId, department);
        }

        public async Task DeleteAsync(int id)
        {
            await _departments.DeleteOneAsync(d => d.DepartmentId == id);
        }

        public async Task<bool> ExistsAsync(string departmentName)
        {
            return await _departments.Find(d => d.DepartmentName == departmentName).AnyAsync();
        }
    }
}
