using TestAPI.Entities;

namespace TestAPI.Interfaces.Repositories
{
    public interface IDepartmentMongoDBRepository
    {
        Task<IEnumerable<DepartmenMongoDB>> GetAllAsync();
        Task<DepartmenMongoDB?> GetByIdAsync(int id);
        Task AddAsync(DepartmenMongoDB department);
        Task UpdateAsync(DepartmenMongoDB department);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(string departmentName);
    }
}
