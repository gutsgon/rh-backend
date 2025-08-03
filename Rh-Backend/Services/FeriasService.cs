
using AutoMapper;
using Rh_Backend.DTO;
using Rh_Backend.Models;
using Rh_Backend.Repository.Interfaces;
using Rh_Backend.Services.Interfaces;

namespace Rh_Backend.Services
{
    public class FeriasService : IFeriasService
    {
        private readonly IFeriasRepository _feriasRepository;
        private readonly IMapper _mapper;
        private readonly IFuncionarioService _funcionarioService;

        public FeriasService(IFeriasRepository feriasRepository, IMapper mapper, IFuncionarioService funcionarioService)
        {
            _feriasRepository = feriasRepository;
            _mapper = mapper;
            _funcionarioService = funcionarioService;
        }

        public async Task<IEnumerable<FeriasReadDTO>> ListarTodasFerias()
        {
            try
            {
                var ferias = await _feriasRepository.GetAllAsync() ?? throw new Exception("Nenhum registro de férias encontrado.");
                return _mapper.Map<IEnumerable<FeriasReadDTO>>(ferias);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao listar férias");
            }
        }

        public async Task<FeriasReadDTO?> BuscarFeriasPorId(long id)
        {
            try
            {
                var ferias = await _feriasRepository.GetByIdAsync(id) ?? throw new Exception("Registro de férias não encontrado.");
                return _mapper.Map<FeriasReadDTO>(ferias);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao buscar férias por ID");
            }
        }

        public async Task<IEnumerable<FeriasReadDTO>> BuscarFeriasPorIdFuncionario(long idFuncionario)
        {
            try
            {
                var ferias = await _feriasRepository.GetByIdFuncionarioAsync(idFuncionario) ?? throw new Exception("Nenhum registro de férias encontrado para o funcionário.");
                return _mapper.Map<IEnumerable<FeriasReadDTO>>(ferias);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao buscar férias por ID do funcionário");
            }
        }

        public async Task<IEnumerable<FeriasReadDTO>> BuscarFeriasPorData(DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                var ferias = await _feriasRepository.GetByDataAsync(dataInicio, dataFim) ?? throw new Exception("Nenhum registro de férias encontrado nesse período.");
                return _mapper.Map<IEnumerable<FeriasReadDTO>>(ferias);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao buscar férias por data");
            }
        }

        public async Task<IEnumerable<FeriasReadDTO>> BuscarFeriasPorStatus(string status)
        {
            try
            {
                var ferias = await _feriasRepository.GetByStatusAsync(status) ?? throw new Exception("Nenhum registro de férias encontrado com esse status.");
                return _mapper.Map<IEnumerable<FeriasReadDTO>>(ferias);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao buscar férias por status");
            }
        }

        public async Task<IEnumerable<FeriasReadDTO>> BuscarFeriasPorFuncionarioEStatus(long idFuncionario, string status)
        {
            try
            {
                var ferias = await _feriasRepository.GetByFuncionarioAndStatusAsync(idFuncionario, status) ?? throw new Exception("Nenhum registro de férias encontrado para o funcionário com esse status.");
                return _mapper.Map<IEnumerable<FeriasReadDTO>>(ferias);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao buscar férias por funcionário e status");
            }
        }

        public async Task<FeriasReadDTO> CriarFerias(FeriasCreateDTO ferias)
        {
            try
            {
                if (ferias == null) throw new Exception("Dados de férias não podem ser nulos.");
                if (ferias.IdFuncionario <= 0) throw new Exception("ID do funcionário inválido.");
                if (ferias.DataInicio >= ferias.DataFim) throw new Exception("Data de início deve ser anterior à data de fim.");
                var funcionario = await _funcionarioService.BuscarFuncionarioPorId(ferias.IdFuncionario) ?? throw new Exception("Funcionário não existe.");

                var feriasModel = _mapper.Map<FeriasModel>(ferias);
                if (ferias.DataInicio > DateTime.Now) feriasModel.Status = "Pendente";
                if (DateTime.Now >= ferias.DataInicio && DateTime.Now <= ferias.DataFim) feriasModel.Status = "Em Andamento";
                if (ferias.DataFim < DateTime.Now) feriasModel.Status = "Concluídas";


                var novasFerias = await _feriasRepository.CreateAsync(feriasModel);
                return _mapper.Map<FeriasReadDTO>(novasFerias);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao criar férias");
            }
        }

        public async Task<FeriasReadDTO> AtualizarFerias(FeriasUpdateDTO ferias)
        {
            try
            {
                if (ferias == null) throw new Exception("Dados de férias não podem ser nulos.");
                if (ferias.Id <= 0) throw new Exception("ID de férias inválido.");
                if (ferias.DataInicio >= ferias.DataFim) throw new Exception("Data de início deve ser anterior à data de fim.");
                var funcionario = await _funcionarioService.BuscarFuncionarioPorId(ferias.IdFuncionario) ?? throw new Exception("Funcionário não existe.");

                var feriasModel = _mapper.Map<FeriasModel>(ferias);
                var novasFerias = await _feriasRepository.UpdateAsync(feriasModel);
                return _mapper.Map<FeriasReadDTO>(novasFerias);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao atualizar férias");
            }
        }

        public async Task<bool> DeletarFerias(long id)
        {
            try
            {
                if (id <= 0) throw new Exception("ID de férias inválido.");
                var ferias = await _feriasRepository.GetByIdAsync(id) ?? throw new Exception("Registro de férias não encontrado.");
                return await _feriasRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao deletar férias");
            }
        }

        public async Task<bool> Exists(long id)
        {
            try
            {
                if (id <= 0) throw new Exception("ID de férias inválido.");
                return await _feriasRepository.ExistsAsync(id);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao verificar existência de férias");
            }
        }  
    }
}