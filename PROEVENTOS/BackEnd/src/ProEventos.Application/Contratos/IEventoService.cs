using ProEventos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application.Contratos
{
    public interface IEventoService
    {
        Task<Evento> AddEvento(Evento model);
        Task<Evento> UpdateEvento(int eventoId, Evento model);
        Task<bool> DeleteEvento(int eventoId);
        Task<Evento[]> GetAllEventosAsysnc(bool includePalestrantes = false);
        Task<Evento[]> GetAllEventosByTemaAsysnc(string tema, bool includePalestrantes = false);
        Task<Evento> GetEventoByIdAsysnc(int EventoId, bool includePalestrantes = false);
    }
}
