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
            bool isExist = await _service.IsDepartmentExist(deptDto.DepartmentName);
            if (isExist)
                return BadRequest("Department Name Already Exist.");

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

        bool isExist = await _service.IsDepartmentExist(deptDto.DepartmentName);
        if (isExist)
            return BadRequest("Department Name Already Exist.");

        try
        {
            var updated = await _service.UpdateAsync(DepartmentId, deptDto);
            if (!updated)
                return NotFound();

            return Ok("Succesfully Updated Department");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
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

            return Ok("Succesfully Deleted Department");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
