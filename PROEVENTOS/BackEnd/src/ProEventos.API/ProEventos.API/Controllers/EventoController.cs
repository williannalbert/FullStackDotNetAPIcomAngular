using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;


namespace ProEventos.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class EventoController : ControllerBase
{
    private readonly IEventoService _eventoService;
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IUserService _userService;

    public EventoController(IEventoService eventoService, 
        IWebHostEnvironment hostEnvironment, 
        IUserService userService)
    {
        _eventoService = eventoService;
        _hostEnvironment = hostEnvironment;
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            EventoDto[] eventos = await _eventoService.GetAllEventosAsysnc(User.GetUserId(), true);
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
            EventoDto evento = await _eventoService.GetEventoByIdAsysnc(User.GetUserId(), id, true);
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
            EventoDto[] evento = await _eventoService.GetAllEventosByTemaAsysnc(User.GetUserId(), tema, true);
            if (evento == null) return NotFound("Nenhum evento encontrado");

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
                DeletarImagem(evento.ImgUrl);
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
                DeletarImagem(evento.ImgUrl);
                evento.ImgUrl = await SalvarImagem(file);
            }

            EventoDto EventoRetorno = await _eventoService.UpdateEvento(User.GetUserId(), eventoId, evento);

            return Ok(EventoRetorno);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao adicionar evento. Erro: " + ex.Message);
        }
    }
    [NonAction]
    public void DeletarImagem(string nomeImagem)
    {
        var caminhoImagem = Path.Combine(_hostEnvironment.ContentRootPath, @"Recursos/Imagens", nomeImagem);
        if (System.IO.File.Exists(caminhoImagem))
            System.IO.File.Delete(caminhoImagem);
    }
    [NonAction]
    public async Task<string> SalvarImagem(IFormFile arquivoImagem)
    {
        string nomeImagem = new String(
            Path.GetFileNameWithoutExtension(arquivoImagem.FileName)
            .Take(10)
            .ToArray()
            ).Replace(' ', '-');

        nomeImagem = $"{nomeImagem}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(arquivoImagem.FileName)}";

        string caminhoImagem = Path.Combine(_hostEnvironment.ContentRootPath, @"Recursos/Imagens", nomeImagem);

        using (var fileStream = new FileStream(caminhoImagem, FileMode.Create))
        {
            await arquivoImagem.CopyToAsync(fileStream);
        }
        return nomeImagem;
    }
}