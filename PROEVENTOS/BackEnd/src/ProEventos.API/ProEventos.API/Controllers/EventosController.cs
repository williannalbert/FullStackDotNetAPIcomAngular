using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.API.Helpers;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Persistence.Models;

namespace ProEventos.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class EventoController : ControllerBase
{
    private readonly IEventoService _eventoService;
    private readonly IUtil _util;
    private readonly IUserService _userService;
    private readonly string _destino = "Imagens";

    public EventoController(IEventoService eventoService, 
        IUtil util, 
        IUserService userService)
    {
        _eventoService = eventoService;
        _util = util;
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery]PageParams pageParams)
    {
        try
        {
            PageList<EventoDto> eventos = await _eventoService.GetAllEventosAsysnc(User.GetUserId(), pageParams, true);
            if(eventos == null) return NotFound("Nenhum evento encontrado");

            //AddPagination criado em Extensions
            Response.AddPagination(eventos.CurrentPage, eventos.PageSize, eventos.TotalCount, eventos.TotalPage);

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
            EventoDto evento = await _eventoService.GetEventoByIdAsysnc(User.GetUserId(), id, true);
            if (evento == null) return NotFound("Evento encontrado");

            return Ok(evento);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao consultar eventos. Erro: " + ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post(EventoDto eventoModel)
    {
        try
        {
            EventoDto evento = await _eventoService.AddEvento(User.GetUserId(), eventoModel);
            if (evento == null) return BadRequest("Erro ao adicionar evento");

            return Ok(evento);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao adicionar evento. Erro: " + ex.Message);
        }
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, EventoDto eventoModel)
    {
        try
        {
            EventoDto evento = await _eventoService.UpdateEvento(User.GetUserId(), id, eventoModel);
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
            EventoDto evento = await _eventoService.GetEventoByIdAsysnc(User.GetUserId(), id, true);
            if (evento == null)
                return NoContent();

            if(await _eventoService.DeleteEvento(User.GetUserId(), id))
            {
                _util.DeleteImage(evento.ImgUrl, _destino);
                return Ok(new { message = "Exclusão realizada com sucesso" });
            }
             else
                throw new Exception("Ocorreu um erro ao excluir Evento");

        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao excluir evento. Erro: " + ex.Message);
        }
    }
    [HttpPost("upload-imagem/{eventoId}")]
    public async Task<IActionResult> UploadImagem(int eventoId)
    {
        try
        {
            EventoDto evento = await _eventoService.GetEventoByIdAsysnc(User.GetUserId(), eventoId, true);
            if (evento == null) return NoContent();

            IFormFile file = Request.Form.Files[0];
            if(file.Length > 0)
            {
                _util.DeleteImage(evento.ImgUrl, _destino);
                evento.ImgUrl = await _util.SaveImage(file, _destino);
            }

            EventoDto EventoRetorno = await _eventoService.UpdateEvento(User.GetUserId(), eventoId, evento);

            return Ok(EventoRetorno);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao adicionar imagem ao evento. Erro: " + ex.Message);
        }
    }
}