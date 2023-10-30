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
        Task<Evento[]> GetAllEventosByTemaAsysnc(string tema, bool includePalestrantes = false);
        Task<Evento[]> GetAllEventosAsysnc(bool includePalestrantes = false);
        Task<Evento> GetEventoByIdAsysnc(int eventoId, bool includePalestrantes = false);
    }
}
