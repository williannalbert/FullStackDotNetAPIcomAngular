using Microsoft.AspNetCore.Mvc;
using ProEventos.Application;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contextos;

namespace ProEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoteController : ControllerBase
{
    private readonly ILoteService _loteService;

    public LoteController(ILoteService loteService)
    {
        _loteService = loteService;
    }

    [HttpGet("{eventoId}")]
    public async Task<IActionResult> Get(int eventoId)
    {
        try
        {
            LoteDto[] lotes = await _loteService.GetLotesByEventoIdAsysnc(eventoId);
            if(lotes == null) return NotFound("Nenhum lote encontrado");

            return Ok(lotes);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao consultar lotes. Erro: "+ex.Message);
        }
    }

    [HttpPut("{eventoId}")]
    public async Task<IActionResult> SaveLotes(int eventoId, LoteDto[] models)
    {
        try
        {
            LoteDto[] lotes = await _loteService.SaveLotes(eventoId, models);
            if (lotes == null) return BadRequest("Erro ao editar lote");

            return Ok(lotes);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao salvar/editar lotes. Erro: " + ex.Message);
        }
    }
    [HttpDelete("{eventoId}/{loteId}")]
    public async Task<IActionResult> Delete(int eventoId, int loteId)
    {
        try
        {
            var lote = await _loteService.GetLoteByIdsAsysnc(eventoId,loteId);
            if (lote == null)
                return NoContent();

            return await _loteService.DeleteLote(lote.EventoId, lote.Id)
                ? Ok(new {message = "Lote excluído com sucesso"})
                : throw new Exception("Ocorreu um erro ao excluir lote");
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao excluir evento. Erro: " + ex.Message);
        }
    }
}