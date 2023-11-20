using ProEventos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Contratos
{
    public interface ILotePersist
    {
        /// <summary>
        /// Método get para retornar lotes por evento
        /// </summary>
        /// <param name="eventoId">Código do evento</param>
        /// <returns>Array de lotes</returns>
        Task<Lote[]> GetLotesByEventoIdAsysnc(int eventoId);
        /// <summary>
        /// Método get para retornar apenas um lote
        /// </summary>
        /// <param name="eventoId">Código do evento</param>
        /// <param name="loteId">Código do lote</param>
        /// <returns>Apenas um lote</returns>
        Task<Lote> GetLoteByIdsAsysnc(int eventoId, int loteId);
    }
}
