using ProEventos.Domain;
using ProEventos.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Contratos
{
    public interface IEventoPersist
    {
        //Task<PageList<Evento>> GetAllEventosByTemaAsysnc(int userId, PageParams pageParams, string tema, bool includePalestrantes = false);
        Task<PageList<Evento>> GetAllEventosAsysnc(int userId, PageParams pageParams, bool includePalestrantes = false);
        Task<Evento> GetEventoByIdAsysnc(int userId, int eventoId, bool includePalestrantes = false);
    }
}
