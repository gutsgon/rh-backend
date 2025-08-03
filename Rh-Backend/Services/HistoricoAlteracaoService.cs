using AutoMapper;
using Rh_Backend.DTO;
using Rh_Backend.Models;
using Rh_Backend.Repository.Interfaces;
using Rh_Backend.Services.Interfaces;

namespace Rh_Backend.Services
{
    public class HistoricoAlteracaoService : IHistoricoAlteracaoService
    {
        private readonly IHistoricoAlteracaoRepository _historicoRepository;
        private readonly IFuncionarioService _funcionarioService;
        private readonly ICargoService _cargoService;
        private readonly IFeriasService _feriasService;
        private readonly IMapper _mapper;
        public HistoricoAlteracaoService(IHistoricoAlteracaoRepository historicoRepository, IFuncionarioService funcionarioService, ICargoService cargoService, IFeriasService feriasService, IMapper mapper)
        {
            _historicoRepository = historicoRepository;
            _mapper = mapper;
            _funcionarioService = funcionarioService;
            _cargoService = cargoService;
            _feriasService = feriasService;
        }

        public async Task<IEnumerable<HistoricoAlteracaoCreateDTO>> ListarTodosHistoricos()
        {
            try
            {
                var historicos = await _historicoRepository.GetAllAsync() ?? throw new Exception("Nenhum histórico encontrado.");
                return _mapper.Map<IEnumerable<HistoricoAlteracaoCreateDTO>>(historicos);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao listar todos os históricos de alteração.", ex);
            }
        }

        public async Task<HistoricoAlteracaoReadDTO?> BuscarHistoricoPorId(long id)
        {
            try
            {
                if (id <= 0) throw new Exception("ID inválido.");
                var historico = await _historicoRepository.GetByIdAsync(id) ?? throw new Exception("Histórico não encontrado.");
                return _mapper.Map<HistoricoAlteracaoReadDTO>(historico);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception($"Erro ao buscar histórico com ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<HistoricoAlteracaoReadDTO>> BuscarHistoricos(long idFuncionario, long? idCargo, long? idFerias)
        {
            try
            {
                if (idFuncionario <= 0 | idCargo <= 0 | idFerias <= 0) throw new Exception("ID(s) inválido(s).");
                var historicos = await _historicoRepository.SearchByIdAsync(idFuncionario, idCargo, idFerias);
                return _mapper.Map<IEnumerable<HistoricoAlteracaoReadDTO>>(historicos);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao buscar históricos de alteração.", ex);
            }
        }

        public async Task<IEnumerable<HistoricoAlteracaoReadDTO>> BuscarHistoricoPorData(DateTime dataAlteracao)
        {
            try
            {
                if (dataAlteracao == default) throw new Exception("Data inválida.");
                if (dataAlteracao > DateTime.Now) throw new Exception("Data de alteração inválida.");
                var historicos = await _historicoRepository.GetByDataAlteracaoAsync(dataAlteracao);
                return _mapper.Map<IEnumerable<HistoricoAlteracaoReadDTO>>(historicos);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao buscar histórico por data.", ex);
            }
        }

        public async Task<HistoricoAlteracaoReadDTO> CriarHistorico(HistoricoAlteracaoCreateDTO historico)
        {
            try
            {
                if (historico == null) throw new Exception("Histórico não pode ser nulo.");
                if (historico.IdFuncionario <= 0 || historico.IdCargo <= 0 || historico.IdFerias <= 0) throw new Exception("IDs inválidos no histórico.");
                if (historico.DataAlteracao > DateTime.Now | historico.CampoAlterado == null | historico.ValorAntigo == null | historico.ValorNovo == null) throw new Exception("Campo(s) inválido(s).");
                if (historico.ValorAntigo == historico.ValorNovo) throw new Exception("Valor antigo e novo não podem ser iguais.");
                var funcionario = await _funcionarioService.BuscarFuncionarioPorId(historico.IdFuncionario) ?? throw new Exception("Funcionário não existe.");
                var cargo = await _cargoService.BuscarCargoPorId(historico.IdCargo) ?? throw new Exception("Cargo não existe.");
                var ferias = await _feriasService.BuscarFeriasPorId(historico.IdFerias) ?? throw new Exception("Férias não existem.");
                var historicoModel = _mapper.Map<HistoricoAlteracaoModel>(historico);
                var novoHistorico = await _historicoRepository.CreateAsync(historicoModel);
                return _mapper.Map<HistoricoAlteracaoReadDTO>(novoHistorico);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao criar histórico de alteração.", ex);
            }
        }
        
        public async Task<bool> DeletarHistorico(long id)
        {
            try
            {
                if (id <= 0) throw new Exception("ID inválido.");
                var historico = await _historicoRepository.GetByIdAsync(id) ?? throw new Exception("Histórico não encontrado.");
                return await _historicoRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception($"Erro ao deletar histórico com ID {id}.", ex);
            }
        }

    }
}