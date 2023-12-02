using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contextos;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Persistence
{
    public class PalestrantePersist : IPalestrantePersist
    {
        private readonly ProEventosContext _context;

        public PalestrantePersist(ProEventosContext context)
        {
            _context = context;
        }
        public async Task<Palestrante[]> GetAllPalestrantesAsysnc(bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(p => p.RedesSociais);

            //query de palestrante-evento dessa forma devido a relação n-n
            if (includeEventos)
                query = query.Include(p => p.PalestrantesEventos).ThenInclude(pe => pe.Evento);

            query = query.AsNoTracking().OrderBy(p => p.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesByNomeAsysnc(string nome, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(p => p.RedesSociais);

            //query de palestrante-evento dessa forma devido a relação n-n
            if (includeEventos)
                query = query.Include(p => p.PalestrantesEventos).ThenInclude(pe => pe.Evento);

            query = query.AsNoTracking().OrderBy(p => p.Id)
                .Where(p => p.User.PrimeiroNome.ToLower().Contains(nome.ToLower())
                && p.User.UltimoNome.ToLower().Contains(nome.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante> GetPalestranteByIdAsysnc(int palestranteId, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(p => p.RedesSociais);

            //query de palestrante-evento dessa forma devido a relação n-n
            if (includeEventos)
                query = query.Include(p => p.PalestrantesEventos).ThenInclude(pe => pe.Evento);

            query = query.AsNoTracking().OrderBy(p => p.Id).Where(p => p.Id == palestranteId);

            return await query.FirstOrDefaultAsync();
        }
    }
}
