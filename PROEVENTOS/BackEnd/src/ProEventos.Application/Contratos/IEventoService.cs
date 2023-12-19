using ProEventos.Application.Dtos;
using ProEventos.Persistence.Models;

namespace ProEventos.Application.Contratos
{
    public interface IEventoService
    {
        Task<EventoDto> AddEvento(int userId, EventoDto model);
        Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto model);
        Task<bool> DeleteEvento(int userId, int eventoId);
        Task<PageList<EventoDto>> GetAllEventosAsysnc(int userId, PageParams pageParams, bool includePalestrantes = false);
        //Task<EventoDto[]> GetAllEventosByTemaAsysnc(int userId, string tema, bool includePalestrantes = false);
        Task<EventoDto> GetEventoByIdAsysnc(int userId, int EventoId, bool includePalestrantes = false);
    }
}
