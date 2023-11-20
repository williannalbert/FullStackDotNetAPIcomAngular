using AutoMapper;
using Microsoft.Extensions.Logging;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application
{
    public class LoteService : ILoteService
    {
        private readonly IGeralPersist _geralPersist;
        private readonly ILotePersist _lotePersist;
        private readonly IMapper _mapper;

        public LoteService(IGeralPersist geralPersist,
            ILotePersist lotePersist,
            IMapper mapper)
        {
            _geralPersist = geralPersist;
            _lotePersist = lotePersist;
            _mapper = mapper;
        }

        public async Task AddEvento(int eventoId, LoteDto model)
        {
            try
            {
                Lote lote = _mapper.Map<Lote>(model);
                lote.EventoId = eventoId;

                _geralPersist.Add<Lote>(lote);
                await _geralPersist.SaveChangesAsysnc();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto[]> SaveLotes(int eventoId, LoteDto[] models)
        {
            try
            {
                var lotes = await _lotePersist.GetLotesByEventoIdAsysnc(eventoId);
                if (lotes == null) return null;

                foreach(LoteDto model in models)
                {
                    if(model.Id == 0)
                    {
                        await AddEvento(eventoId, model);
                    }
                    else
                    {
                        var lote = lotes.FirstOrDefault(lote => lote.Id == model.Id);
                        model.EventoId = eventoId;

                        _mapper.Map(model, lote);

                        _geralPersist.Update<Lote>(lote);
                        await _geralPersist.SaveChangesAsysnc();
                    }
                }
                var lotesRetorno = await _lotePersist.GetLotesByEventoIdAsysnc(eventoId);

                return _mapper.Map<LoteDto[]>(lotesRetorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteLote(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotePersist.GetLoteByIdsAsysnc(eventoId, loteId);
                if (lote == null)
                    throw new Exception("Lote não encontrado");

                _geralPersist.Delete<Lote>(lote);
                return await _geralPersist.SaveChangesAsysnc();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto[]> GetLotesByEventoIdAsysnc(int EventoId)
        {
            try
            {
                Lote[] lotes = await _lotePersist.GetLotesByEventoIdAsysnc(EventoId);
                if (lotes == null)
                    return null;

                LoteDto[] resultado = _mapper.Map<LoteDto[]>(lotes);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); ;
            }
        }

        public async Task<LoteDto> GetLoteByIdsAsysnc(int eventoId, int loteId)
        {
            try
            {
                Lote lote = await _lotePersist.GetLoteByIdsAsysnc(eventoId, loteId);
                if (lote == null)
                    return null;

                LoteDto resultado = _mapper.Map<LoteDto>(lote);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); ;
            }
        }

    }
}
