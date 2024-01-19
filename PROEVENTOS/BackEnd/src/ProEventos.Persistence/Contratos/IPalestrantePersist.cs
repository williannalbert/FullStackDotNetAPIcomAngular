using ProEventos.Domain;
using ProEventos.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Contratos
{
    public interface IPalestrantePersist : IGeralPersist
    {
        Task<PageList<Palestrante>> GetAllPalestrantesAsysnc(PageParams pageParams, bool includeEventos = false);
        Task<Palestrante> GetPalestranteByIdAsysnc(int userId, bool includeEventos = false);
    }
}
