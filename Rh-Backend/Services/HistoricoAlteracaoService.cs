using AutoMapper;
using Rh_Backend.DTO;
using Rh_Backend.Exceptions;
using Rh_Backend.Models;
using Rh_Backend.Repository.Interfaces;
using Rh_Backend.Services.Interfaces;

namespace Rh_Backend.Services
{
    public class HistoricoAlteracaoService : IHistoricoAlteracaoService
    {
        private readonly IHistoricoAlteracaoRepository _historicoRepository;
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly ICargoRepository _cargoRepository;
        private readonly IFeriasRepository _feriasRepository;
        private readonly IMapper _mapper;
        public HistoricoAlteracaoService(IHistoricoAlteracaoRepository historicoRepository, IFuncionarioRepository funcionarioRepository, ICargoRepository cargoRepository, IFeriasRepository feriasRepository, IMapper mapper)
        {
            _historicoRepository = historicoRepository;
            _mapper = mapper;
            _funcionarioRepository = funcionarioRepository;
            _cargoRepository = cargoRepository;
            _feriasRepository = feriasRepository;
        }

        public async Task<IEnumerable<HistoricoAlteracaoCreateDTO>> ListarTodosHistoricos()
        {
            try
            {
                var historicos = await _historicoRepository.GetAllAsync() ?? throw new NotFoundException("Nenhum histórico encontrado.");
                return _mapper.Map<IEnumerable<HistoricoAlteracaoCreateDTO>>(historicos);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<HistoricoAlteracaoReadDTO?> BuscarHistoricoPorId(long id)
        {
            try
            {
                if (id <= 0) throw new BadRequestException("ID inválido.");
                var historico = await _historicoRepository.GetByIdAsync(id) ?? throw new NotFoundException("Histórico não encontrado.");
                return _mapper.Map<HistoricoAlteracaoReadDTO>(historico);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<HistoricoAlteracaoReadDTO>> BuscarHistoricos(long idFuncionario, long? idCargo, long? idFerias)
        {
            try
            {
                if (idFuncionario <= 0 | idCargo <= 0 | idFerias <= 0) throw new BadRequestException("ID(s) inválido(s).");
                var historicos = await _historicoRepository.SearchByIdAsync(idFuncionario, idCargo, idFerias);
                return _mapper.Map<IEnumerable<HistoricoAlteracaoReadDTO>>(historicos);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<HistoricoAlteracaoReadDTO>> BuscarHistoricoPorData(DateTime dataAlteracao)
        {
            try
            {
                if (dataAlteracao == default) throw new BadRequestException("Data inválida.");
                if (dataAlteracao > DateTime.Now) throw new BadRequestException("Data de alteração inválida.");
                var historicos = await _historicoRepository.GetByDataAlteracaoAsync(dataAlteracao);
                return _mapper.Map<IEnumerable<HistoricoAlteracaoReadDTO>>(historicos);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<HistoricoAlteracaoReadDTO> CriarHistorico(HistoricoAlteracaoCreateDTO historico)
        {
            try
            {
                if (historico == null) throw new Exception("Histórico não pode ser nulo.");
                if (historico.IdFuncionario <= 0 || historico.IdCargo <= 0 || historico.IdFerias <= 0) throw new BadRequestException("IDs inválidos no histórico.");
                if (historico.DataAlteracao > DateTime.Now | historico.CampoAlterado == null | historico.ValorAntigo == null | historico.ValorNovo == null) throw new BadRequestException("Campo(s) inválido(s).");
                if (historico.ValorAntigo == historico.ValorNovo) throw new Exception("Valor antigo e novo não podem ser iguais.");
                var funcionario = await _funcionarioRepository.GetByIdAsync(historico.IdFuncionario) ?? throw new NotFoundException("Funcionário não existe.");
                var cargo = await _cargoRepository.GetByIdAsync(historico.IdCargo) ?? throw new NotFoundException("Cargo não existe.");
                var ferias = await _feriasRepository.GetByIdAsync(historico.IdFerias) ?? throw new NotFoundException("Férias não existem.");
                var historicoModel = _mapper.Map<HistoricoAlteracaoModel>(historico);
                var novoHistorico = await _historicoRepository.CreateAsync(historicoModel);
                return _mapper.Map<HistoricoAlteracaoReadDTO>(novoHistorico);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<bool> DeletarHistorico(long id)
        {
            try
            {
                if (id <= 0) throw new Exception("ID inválido.");
                var historico = await _historicoRepository.GetByIdAsync(id) ?? throw new NotFoundException("Histórico não encontrado.");
                return await _historicoRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<bool> Exists(long id)
        {
            try
            {
                if (id <= 0) throw new BadRequestException("ID do historico deve ser maior que zero.");
                var historico = await _historicoRepository.GetByIdAsync(id) ?? throw new NotFoundException("Historico não encontrado.");
                if (historico == null) return false;
                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

    }
}