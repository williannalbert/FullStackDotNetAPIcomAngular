using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contratos;
using ProEventos.Domain;
using ProEventos.Persistence.Contextos;

namespace ProEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventoController : ControllerBase
{
    private readonly IEventoService _eventoService;

    public EventoController(IEventoService eventoService)
    {
        _eventoService = eventoService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            Evento[] eventos = await _eventoService.GetAllEventosAsysnc(true);
            if(eventos == null) return NotFound("Nenhum evento encontrado");

            return Ok(eventos);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao consultar eventos. Erro: "+ex.Message);
        }
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            Evento evento = await _eventoService.GetEventoByIdAsysnc(id, true);
            if (evento == null) return NotFound("Evento encontrado");

            return Ok(evento);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao consultar eventos. Erro: " + ex.Message);
        }
    }

    [HttpGet("tema/{tema}")]
    public async Task<IActionResult> GetByTema(string tema)
    {
        try
        {
            Evento[] evento = await _eventoService.GetAllEventosByTemaAsysnc(tema, true);
            if (evento == null) return NotFound("Nenhum evento encontrado");

            return Ok(evento);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao consultar eventos. Erro: " + ex.Message);
        }
    }
    [HttpPost]
    public async Task<IActionResult> Post(Evento eventoModel)
    {
        try
        {
            Evento evento = await _eventoService.AddEvento(eventoModel);
            if (evento == null) return BadRequest("Erro ao adicionar evento");

            return Ok(evento);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao adicionar evento. Erro: " + ex.Message);
        }
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Evento eventoModel)
    {
        try
        {
            Evento evento = await _eventoService.UpdateEvento(id, eventoModel);
            if (evento == null) return BadRequest("Erro ao editar evento");

            return Ok(evento);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao editar evento. Erro: " + ex.Message);
        }
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            if (await _eventoService.DeleteEvento(id))
                return Ok("Exclusão realizada com sucesso");
            else
                return BadRequest("Evento não excluído");
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao excluir evento. Erro: " + ex.Message);
        }
    }
}