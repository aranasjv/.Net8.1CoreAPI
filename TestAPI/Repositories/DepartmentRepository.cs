using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.Entities;
using TestAPI.Interfaces.Repositories;

namespace TestAPI.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _context;
        public DepartmentRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Department>> GetAllAsync()
            => await _context.Departments.ToListAsync();

        public async Task<Department?> GetByIdAsync(int id)
            => await _context.Departments.FindAsync(id);

        public async Task AddAsync(Department department)
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Department department)
        {
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var department = await GetByIdAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(string departmentName)
            => await _context.Departments.AnyAsync(d => d.DepartmentName == departmentName);
    }

}
