using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.Entities;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            try
            {
                return await _context.Employees.ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{EmployeeId}")]
        public async Task<ActionResult<Employee>> GetEmployee(int EmployeeId)
        {
            try
            {
                var Employee = await _context.Employees.FindAsync(EmployeeId);
                if (Employee == null)
                    return NotFound();

                return Employee;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee dept)
        {
            try
            {
                _context.Employees.Add(dept);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetEmployee), new { EmployeeId = dept.EmployeeId }, dept);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{EmployeeId}")]
        public async Task<IActionResult> UpdateEmployee(int EmployeeId, Employee dept)
        {
            if (EmployeeId != dept.EmployeeId)
                return BadRequest("Mismatched EmployeeId.");

            try
            {
                _context.Entry(dept).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{EmployeeId}")]
        public async Task<IActionResult> DeleteEmployee(int EmployeeId)
        {
            try
            {
                var dept = await _context.Employees.FindAsync(EmployeeId);
                if (dept == null)
                    return NotFound();

                _context.Employees.Remove(dept);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SaveFile")]
        public IActionResult SaveFile()
        {
            try
            {
                var file = Request.Form.Files.FirstOrDefault();
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded.");

                var uploadsFolder = Path.Combine(_env.ContentRootPath, "Photos");

                // Ensure the directory exists
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return Ok(new { message = "File uploaded successfully.", fileName = file.FileName });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


    }
}
