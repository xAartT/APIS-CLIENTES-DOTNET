using System;
using System.Threading.Tasks;
using App.Application.DTOs;
using App.Application.Interfaces;
using App.Domain.Error;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace App.Controllers;

[ApiController]
[Route("api/enderecos")]
public class EnderecosController(IEnderecoService service) : ControllerBase
{
    private readonly IEnderecoService _service = service;

    [HttpGet("cliente/{clienteId}")]
    public async Task<IActionResult> GetByCliente(int clienteId) => await HandleAsync(async () =>
    {
        var enderecos = await _service.GetByCliente(clienteId);
        return Ok(enderecos);
    });

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) => await HandleAsync(async () =>
    {
        var endereco = await _service.GetById(id);
        return Ok(endereco);
    });

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EnderecoDto dto) => await HandleAsync(async () =>
    {
        var endereco = await _service.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = endereco.Id }, endereco);
    });

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] EnderecoDto dto) => await HandleAsync(async () =>
    {
        var endereco = await _service.Update(id, dto);
        return Ok(endereco);
    });

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) => await HandleAsync(async () =>
    {
        await _service.Delete(id);
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
