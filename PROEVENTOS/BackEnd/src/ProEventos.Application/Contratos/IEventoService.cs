using ProEventos.Application.Dtos;

namespace ProEventos.Application.Contratos
{
    public interface IEventoService
    {
        Task<EventoDto> AddEvento(EventoDto model);
        Task<EventoDto> UpdateEvento(int eventoId, EventoDto model);
        Task<bool> DeleteEvento(int eventoId);
        Task<EventoDto[]> GetAllEventosAsysnc(bool includePalestrantes = false);
        Task<EventoDto[]> GetAllEventosByTemaAsysnc(string tema, bool includePalestrantes = false);
        Task<EventoDto> GetEventoByIdAsysnc(int EventoId, bool includePalestrantes = false);
    }
}
