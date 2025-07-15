// Controllers/DepartmentController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAPI.Data;
using TestAPI.DTOs;
using TestAPI.Entities;
using TestAPI.Interfaces.Services;


[ApiController]
[Route("api/[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _service;

    public DepartmentController(IDepartmentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments()
    {
        try
        {
            var departments = await _service.GetAllAsync();
            return Ok(departments);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{DepartmentId}")]
    public async Task<ActionResult<DepartmentDto>> GetDepartment(int DepartmentId)
    {
        try
        {
            var department = await _service.GetByIdAsync(DepartmentId);
            if (department == null)
                return NotFound();

            return Ok(department);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<DepartmentDto>> CreateDepartment(DepartmentDto deptDto)
    {
        try
        {
            var created = await _service.AddAsync(deptDto);
            return CreatedAtAction(nameof(GetDepartment), new { DepartmentId = created.DepartmentId }, created);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{DepartmentId}")]
    public async Task<IActionResult> UpdateDepartment(int DepartmentId, DepartmentDto deptDto)
    {
        if (DepartmentId != deptDto.DepartmentId)
            return BadRequest("Mismatched DepartmentId.");

        try
        {
            var existing = await _service.GetByIdAsync(DepartmentId);
            if (existing == null)
                return NotFound();

            // Only check for name existence if the name is being changed
            if (!string.Equals(existing.DepartmentName, deptDto.DepartmentName, StringComparison.OrdinalIgnoreCase))
            {
                bool isExist = await _service.IsDepartmentExist(deptDto.DepartmentName);
                if (isExist)
                    return BadRequest("Department Name Already Exist.");
            }

            var updated = await _service.UpdateAsync(DepartmentId, deptDto);
            if (!updated)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpDelete("{DepartmentId}")]
    public async Task<IActionResult> DeleteDepartment(int DepartmentId)
    {
        try
        {
            var deleted = await _service.DeleteAsync(DepartmentId);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}
