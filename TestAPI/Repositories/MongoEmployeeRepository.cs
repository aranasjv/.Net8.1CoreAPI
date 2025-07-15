using MongoDB.Bson;
using MongoDB.Driver;
using TestAPI.Entities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestAPI.Repositories
{
    public class MongoEmployeeRepository
    {
        private readonly IMongoCollection<Employee> _employees;

        public MongoEmployeeRepository(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
            var database = client.GetDatabase(configuration["MongoDbSettings:Database"]);
            _employees = database.GetCollection<Employee>("Employees");
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _employees.Find(_ => true).ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _employees.Find(e => e.EmployeeId == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Employee employee)
        {
            await _employees.InsertOneAsync(employee);
        }

        public async Task UpdateAsync(Employee employee)
        {
            await _employees.ReplaceOneAsync(e => e.EmployeeId == employee.EmployeeId, employee);
        }

        public async Task DeleteAsync(int id)
        {
            await _employees.DeleteOneAsync(e => e.EmployeeId == id);
        }
    }
}
