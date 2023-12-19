using AutoMapper;
using Microsoft.Extensions.Logging;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;
using ProEventos.Persistence.Models;
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
        private readonly IMapper _mapper;

        public EventoService(IGeralPersist geralPersist, 
            IEventoPersist eventoPersist,
            IMapper mapper)
        {
            _geralPersist = geralPersist;
            _eventoPersist = eventoPersist;
            _mapper = mapper;
        }

        public async Task<EventoDto> AddEvento(int userId, EventoDto model)
        {
            try
            {
                Evento evento = _mapper.Map<Evento>(model);
                evento.UserId = userId;

                _geralPersist.Add<Evento>(evento);
                if (await _geralPersist.SaveChangesAsysnc())
                {
                    Evento retorno = await _eventoPersist.GetEventoByIdAsysnc(userId, evento.Id, false);
                    return _mapper.Map<EventoDto>(retorno);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto model)
        {
            try
            {
                Evento evento = await _eventoPersist.GetEventoByIdAsysnc(userId, eventoId, false);
                if (evento == null) return null;

                model.Id = evento.Id;
                model.UserId = userId;

                _mapper.Map(model, evento);

                _geralPersist.Update<Evento>(evento);
                if (await _geralPersist.SaveChangesAsysnc())
                {
                    Evento retorno = await _eventoPersist.GetEventoByIdAsysnc(userId, evento.Id, false);
                    return _mapper.Map<EventoDto>(retorno);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvento(int userId, int eventoId)
        {
            try
            {
                Evento evento = await _eventoPersist.GetEventoByIdAsysnc(userId, eventoId, false);
                if (evento == null)
                    throw new Exception("Evento não encontrado");

                _geralPersist.Delete<Evento>(evento);
                return await _geralPersist.SaveChangesAsysnc();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageList<EventoDto>> GetAllEventosAsysnc(int userId, PageParams pageParams, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosAsysnc(userId, pageParams, includePalestrantes);
                if (eventos == null)
                    return null;

                var resultado = _mapper.Map<PageList<EventoDto>>(eventos);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); ;
            }
        }

        /*public async Task<EventoDto[]> GetAllEventosByTemaAsysnc(int userId, string tema, bool includePalestrantes = false)
        {
            try
            {
                Evento[] eventos = await _eventoPersist.GetAllEventosByTemaAsysnc(userId, tema, includePalestrantes);
                if (eventos == null)
                    return null;

                var resultado = _mapper.Map<EventoDto[]>(eventos);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); ;
            }
        }*/

        public async Task<EventoDto> GetEventoByIdAsysnc(int userId, int eventoId, bool includePalestrantes = false)
        {
            try
            {
                Evento evento = await _eventoPersist.GetEventoByIdAsysnc(userId, eventoId, includePalestrantes);
                if (evento == null)
                    return null;

                var resultado = _mapper.Map<EventoDto>(evento);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); ;
            }
        }

    }
}
