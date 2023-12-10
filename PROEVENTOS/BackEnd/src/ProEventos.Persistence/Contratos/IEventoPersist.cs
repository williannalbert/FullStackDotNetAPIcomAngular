using ProEventos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Contratos
{
    public interface IEventoPersist
    {
        Task<Evento[]> GetAllEventosByTemaAsysnc(int userId, string tema, bool includePalestrantes = false);
        Task<Evento[]> GetAllEventosAsysnc(int userId, bool includePalestrantes = false);
        Task<Evento> GetEventoByIdAsysnc(int userId, int eventoId, bool includePalestrantes = false);
    }
}
