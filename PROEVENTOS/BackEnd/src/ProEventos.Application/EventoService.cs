using Microsoft.Extensions.Logging;
using ProEventos.Application.Contratos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IGeralPersist _geralPersist;
        private readonly IEventoPersist _eventoPersist;

        public EventoService(IGeralPersist geralPersist, IEventoPersist eventoPersist)
        {
            _geralPersist = geralPersist;
            _eventoPersist = eventoPersist;
        }

        public async Task<Evento> AddEvento(Evento model)
        {
            try
            {
                _geralPersist.Add<Evento>(model);
                if (await _geralPersist.SaveChangesAsysnc())
                    return await _eventoPersist.GetEventoByIdAsysnc(model.Id, false);

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> UpdateEvento(int eventoId, Evento model)
        {
            try
            {
                Evento evento = await _eventoPersist.GetEventoByIdAsysnc(eventoId, false);
                if (evento == null) return null;

                model.Id = evento.Id;

                _geralPersist.Update(model);
                if (await _geralPersist.SaveChangesAsysnc())
                    return await _eventoPersist.GetEventoByIdAsysnc(model.Id, false);

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvento(int eventoId)
        {
            try
            {
                Evento evento = await _eventoPersist.GetEventoByIdAsysnc(eventoId, false);
                if (evento == null)
                    throw new Exception("Evento não encontrado");

                _geralPersist.Delete(evento);
                return await _geralPersist.SaveChangesAsysnc();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosAsysnc(bool includePalestrantes = false)
        {
            try
            {
                Evento[] eventos = await _eventoPersist.GetAllEventosAsysnc(includePalestrantes);
                if (eventos == null)
                    return null;

                return eventos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); ;
            }
        }

        public async Task<Evento[]> GetAllEventosByTemaAsysnc(string tema, bool includePalestrantes = false)
        {
            try
            {
                Evento[] eventos = await _eventoPersist.GetAllEventosByTemaAsysnc(tema, includePalestrantes);
                if (eventos == null)
                    return null;

                return eventos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); ;
            }
        }

        public async Task<Evento> GetEventoByIdAsysnc(int eventoId, bool includePalestrantes = false)
        {
            try
            {
                Evento eventos = await _eventoPersist.GetEventoByIdAsysnc(eventoId, includePalestrantes);
                if (eventos == null)
                    return null;

                return eventos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); ;
            }
        }

    }
}
