using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Models;

namespace ProEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventoController : ControllerBase
{
    public EventoController(ILogger<EventoController> logger)
    {
       
    }

    [HttpGet]
    public IEnumerable<Evento> Get()
    {
        return new Evento[]
        {
            new Evento(){
                EventoId = 1,
                Tema = "Teste",
                Local = "Teste Local" 
            }
        };
    }
}