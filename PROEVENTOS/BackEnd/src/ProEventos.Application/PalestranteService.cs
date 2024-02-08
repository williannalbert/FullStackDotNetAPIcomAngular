using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;
using ProEventos.Persistence.Models;

namespace ProEventos.Application
{
    public class PalestranteService : IPalestranteService
    {
        private readonly IPalestrantePersist _palestrantePersist;
        private readonly IMapper _mapper;

        public PalestranteService(
            IPalestrantePersist palestrantePersist,
            IMapper mapper)
        {
            _palestrantePersist = palestrantePersist;
            _mapper = mapper;
        }

        public async Task<PalestranteDto> AddPalestrante(int userId, PalestranteAddDto model)
        {
            try
            {
                Palestrante palestrante = _mapper.Map<Palestrante>(model);
                palestrante.UserId = userId;

                _palestrantePersist.Add<Palestrante>(palestrante);
                if (await _palestrantePersist.SaveChangesAsysnc())
                {
                    Palestrante retorno = await _palestrantePersist.GetPalestranteByUserIdAsysnc(userId, false);
                    return _mapper.Map<PalestranteDto>(retorno);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteDto> UpdatePalestrante(int userId, PalestranteUpdateDto model)
        {
            try
            {
                Palestrante palestrante = await _palestrantePersist.GetPalestranteByUserIdAsysnc(userId, false);
                if (palestrante == null) return null;

                model.Id = palestrante.Id;
                model.UserId = userId;

                _mapper.Map(model, palestrante);

                _palestrantePersist.Update<Palestrante>(palestrante);
                if (await _palestrantePersist.SaveChangesAsysnc())
                {
                    Palestrante retorno = await _palestrantePersist.GetPalestranteByUserIdAsysnc(userId, false);
                    return _mapper.Map<PalestranteDto>(retorno);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageList<PalestranteDto>> GetAllPalestrantesAsysnc(PageParams pageParams, bool includePalestrantes = false)
        {
            try
            {
                var palestrantes = await _palestrantePersist.GetAllPalestrantesAsysnc(pageParams, includePalestrantes);
                if (palestrantes == null)
                    return null;

                var resultado = _mapper.Map<PageList<PalestranteDto>>(palestrantes);

                resultado.CurrentPage = palestrantes.CurrentPage;
                resultado.TotalPage = palestrantes.TotalPage;
                resultado.PageSize = palestrantes.PageSize;
                resultado.TotalCount = palestrantes.TotalCount;

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); ;
            }
        }
        public async Task<PalestranteDto> GetPalestranteByUserIdAsysnc(int userId, bool includeEventos = false)
        {
            try
            {
                var palestrante = await _palestrantePersist.GetPalestranteByUserIdAsysnc(userId, includeEventos);
                if (palestrante == null)
                    return null;

                var resultado = _mapper.Map<PalestranteDto>(palestrante);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); ;
            }
        }

    }
}
