using Microsoft.AspNetCore.Mvc;
using ProEventos.Persistence;
using ProEventos.Domain;

namespace ProEventos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventoController : ControllerBase
{
    private readonly ProEventosContext _context;
    public EventoController(ProEventosContext context)
    {
       _context = context;
    }

    [HttpGet]
    public IEnumerable<Evento> Get()
    {
        return _context.Eventos;
    }
    [HttpGet("{id}")]
    public Evento GetById(int id)
    {
        return _context.Eventos.FirstOrDefault(evento => evento.Id == id);
    }
}