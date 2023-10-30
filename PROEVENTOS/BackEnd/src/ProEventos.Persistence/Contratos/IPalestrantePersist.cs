using ProEventos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Contratos
{
    public interface IPalestrantePersist
    {
        Task<Palestrante[]> GetAllPalestrantesByNomeAsysnc(string nome, bool includeEventos = false);
        Task<Palestrante[]> GetAllPalestrantesAsysnc(bool includeEventos = false);
        Task<Palestrante> GetPalestranteByIdAsysnc(int palestranteId, bool includeEventos = false);
    }
}
