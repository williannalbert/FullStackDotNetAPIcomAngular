using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contextos;

namespace ProEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RedesSociaisController : ControllerBase
{
    private readonly IRedeSocialService _redesSociaisService;
    private readonly IEventoService _eventoService;
    private readonly IPalestranteService _palestranteService;

    public RedesSociaisController(IRedeSocialService redesSociaisService, 
            IEventoService eventoService,
            IPalestranteService palestranteService)
    {
        _redesSociaisService = redesSociaisService;
        _eventoService = eventoService;
        _palestranteService = palestranteService;
    }

    [HttpGet("evento/{eventoId}")]
    public async Task<IActionResult> GetByEvento(int eventoId)
    {
        try
        {
            if(!(await AutorEvento(eventoId)))
                return Unauthorized();

            RedeSocialDto[] redesSociais = await _redesSociaisService.GetAllByEventoIdAsync(eventoId);
            if(redesSociais == null) return NotFound("Nenhuma rede social encontrada");

            return Ok(redesSociais);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao consultar rede social. Erro: " + ex.Message);
        }
    }

    [HttpGet("palestrante")]
    public async Task<IActionResult> GetByPalestrante()
    {
        try
        {
            var palestrante = await _palestranteService.GetPalestranteByUserIdAsysnc(User.GetUserId());
            if (palestrante == null)
                return Unauthorized();

            RedeSocialDto[] redesSociais = await _redesSociaisService.GetAllByPalestranteIdAsync(palestrante.Id);
            if (redesSociais == null) return NotFound("Nenhuma rede social encontrada");

            return Ok(redesSociais);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao consultar rede social. Erro: " + ex.Message);
        }
    }
    [HttpPut("evento/{eventoId}")]
    public async Task<IActionResult> SaveByEvento(int eventoId, RedeSocialDto[] models)
    {
        try
        {
            if (!(await AutorEvento(eventoId)))
                return Unauthorized();
            
            RedeSocialDto[] redesSociais = await _redesSociaisService.SaveByEvento(eventoId, models);
            if (redesSociais == null) return BadRequest("Erro ao editar redes sociais");

            return Ok(redesSociais);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao salvar/editar redes sociais. Erro: " + ex.Message);
        }
    }

    [HttpPut("palestrante")]
    public async Task<IActionResult> SaveByPalestrante(RedeSocialDto[] models)
    {
        try
        {
            var palestrante = await _palestranteService.GetPalestranteByUserIdAsysnc(User.GetUserId());
            if (palestrante == null)
                return Unauthorized();

            RedeSocialDto[] redesSociais = await _redesSociaisService.SaveByPalestrante(palestrante.Id, models);
            if (redesSociais == null) return BadRequest("Erro ao editar redes sociais");

            return Ok(redesSociais);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao salvar/editar redes sociais. Erro: " + ex.Message);
        }
    }

    [HttpDelete("evento/{eventoId}/{redeSocialId}")]
    public async Task<IActionResult> DeleteByEvento(int eventoId, int redeSocialId)
    {
        try
        {
            if (!(await AutorEvento(eventoId)))
                return Unauthorized();

            var redesSociais = await _redesSociaisService.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
            if (redesSociais == null)
                return NoContent();

            return await _redesSociaisService.DeleteByEvento(eventoId, redeSocialId)
                ? Ok(new {message = "Redes Social excluída com sucesso"})
                : throw new Exception("Ocorreu um erro ao excluir Redes Social");
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao excluir Redes Social. Erro: " + ex.Message);
        }
    }
    [HttpDelete("palestrante/{redeSocialId}")]
    public async Task<IActionResult> DeleteByPalestrante(int redeSocialId)
    {
        try
        {
            var palestrante = await _palestranteService.GetPalestranteByUserIdAsysnc(User.GetUserId());
            if (palestrante == null)
                return Unauthorized();

            var redesSociais = await _redesSociaisService.GetRedeSocialPalestranteByIdsAsync(palestrante.Id, redeSocialId);
            if (redesSociais == null)
                return NoContent();

            return await _redesSociaisService.DeleteByPalestrante(palestrante.Id, redeSocialId)
                ? Ok(new { message = "Redes Social excluída com sucesso" })
                : throw new Exception("Ocorreu um erro ao excluir Redes Social");
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao excluir Redes Social. Erro: " + ex.Message);
        }
    }

    [NonAction]
    private async Task<bool> AutorEvento(int eventoId)
    {
        var evento = await _eventoService.GetEventoByIdAsysnc(User.GetUserId(), eventoId, false);
        if (evento == null)
            return false;

        return true;
    }
}