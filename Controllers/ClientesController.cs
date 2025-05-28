using System;
using System.Threading.Tasks;
using App.Application.DTOs;
using App.Application.Interfaces;
using App.Domain.Error;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace App.Controllers;

[ApiController]
[Route("api/clientes")]
public class ClientesController(IClienteService service) : ControllerBase
{
    private readonly IClienteService _service = service;

    [HttpGet]
    public async Task<IActionResult> Get() => await HandleAsync(async () => 
    {
        var result = await _service.GetAll();
        return Ok(result);
    });

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) => await HandleAsync(async () =>
    {
        var cliente = await _service.GetById(id);
        return  Ok(cliente);
    });

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ClienteDto dto) => await HandleAsync(async () =>
    {
        var created = await _service.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    });

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => await HandleAsync(async () =>
    {
        var deleted = await _service.Delete(id);
        return NoContent();
    });

    private async Task<IActionResult> HandleAsync(Func<Task<IActionResult>> action)
    {
        try
        {
            return await action();
        }
        catch (AppException ex)
        {
            return StatusCode(ex.StatusCode, new
            {
                error = new
                {
                    code = ex.ErrorCode,
                    message = ex.Message
                }
            });
        }
        catch (Exception ex)
        {
                Console.WriteLine(ex);
                return StatusCode(500, new
                {
                    error = new
                    {
                        code = "internal_server_error",
                        message = "An unexpected error occurred."
                    }
                });
        }
    }
}
