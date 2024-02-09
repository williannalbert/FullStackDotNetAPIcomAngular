using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Persistence.Models;

namespace ProPalestrantess.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class PalestrantesController : ControllerBase
{
    private readonly IPalestranteService _palestrantesService;
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IUserService _userService;

    public PalestrantesController(IPalestranteService palestrantesService, 
        IWebHostEnvironment hostEnvironment, 
        IUserService userService)
    {
        _palestrantesService = palestrantesService;
        _hostEnvironment = hostEnvironment;
        _userService = userService;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll([FromQuery]PageParams pageParams)
    {
        try
        {
            PageList<PalestranteDto> palestrantes = await _palestrantesService.GetAllPalestrantesAsysnc(pageParams, true);
            if(palestrantes == null) return NotFound("Nenhum palestrantes encontrado");

            //AddPagination criado em Extensions
            Response.AddPagination(palestrantes.CurrentPage, palestrantes.PageSize, palestrantes.TotalCount, palestrantes.TotalPage);

            return Ok(palestrantes);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao consultar palestrantes. Erro: "+ex.Message);
        }
    }
    [HttpGet()]
    public async Task<IActionResult> GetPalestrantes()
    {
        try
        {
            PalestranteDto palestrantes = await _palestrantesService.GetPalestranteByUserIdAsysnc(User.GetUserId(), true);
            if (palestrantes == null) return NotFound("Palestrante não encontrado");

            return Ok(palestrantes);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao consultar palestrantes. Erro: " + ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post(PalestranteAddDto palestranteModel)
    {
        try
        {
            PalestranteDto palestrantes = await _palestrantesService.GetPalestranteByUserIdAsysnc(User.GetUserId(), false);

            if (palestrantes == null) 
                palestrantes = await _palestrantesService.AddPalestrante(User.GetUserId(), palestranteModel);

            return Ok(palestrantes);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao adicionar palestrante. Erro: " + ex.Message);
        }
    }
    [HttpPut()]
    public async Task<IActionResult> Put(PalestranteUpdateDto palestranteModel)
    {
        try
        {
            PalestranteDto palestrantes = await _palestrantesService.UpdatePalestrante(User.GetUserId(), palestranteModel);
            if (palestrantes == null) return BadRequest("Erro ao editar palestrante");

            return Ok(palestrantes);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao editar palestrante. Erro: " + ex.Message);
        }
    }
}