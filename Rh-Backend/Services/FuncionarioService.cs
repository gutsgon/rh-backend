using AutoMapper;
using Rh_Backend.DTO;
using Rh_Backend.Repository.Interfaces;
using Rh_Backend.Services.Interfaces;

namespace Rh_Backend.Services
{
    public class FuncionarioService : IFuncionarioService
    {
        private readonly IFuncionarioRepository _repository;
        private readonly IMapper _mapper;
        public FuncionarioService(IFuncionarioRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<IEnumerable<FuncionarioReadDTO>> ListarTodosFuncionarios()
        {
            try
            {
                var funcionarios = await _repository.GetAllAsync() ?? throw new Exception("Nenhum funcionário encontrado");
                var funcionariosDTO = _mapper.Map<IEnumerable<FuncionarioReadDTO>>(funcionarios);
                return funcionariosDTO;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao listar funcionários");
            }
        }

        public async Task<IEnumerable<FuncionarioReadDTO>> BuscarFuncionarios(DateTime? dataAdmissao, string nome, decimal salario, bool status)
        {
            try
            {
                if (dataAdmissao > DateTime.Now) throw new Exception("Data inválida");
                if (nome.Any(char.IsDigit)) throw new Exception("Nome inválido");
                if (salario < 0) throw new Exception("Salário inválido");
                var funcionarios = await _repository.SearchAsync(dataAdmissao, nome, salario, status);
                return _mapper.Map<IEnumerable<FuncionarioReadDTO>>(funcionarios);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao buscar funcionários");
            }
        }

        public async Task<IEnumerable<FuncionarioReadDTO>> BuscarFuncionariosPorCargo(string cargo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cargo)) throw new Exception("Cargo inválido");
                var funcionarios = await _repository.GetByCargoAsync(cargo);
                return _mapper.Map<IEnumerable<FuncionarioReadDTO>>(funcionarios);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao buscar funcionários por cargo");
            }
        }

        public async Task<FuncionarioDetalhesDTO> BuscarFuncionarioDetalhado(long id)
        {
            try
            {
                if (id <= 0) throw new Exception("ID inválido");
                if (!await _repository.ExistsAsync(id)) throw new Exception("Funcionário não existe");
                var funcionario = await _repository.GetByIdAsync(id) ?? throw new Exception("Funcionário não encontrado");
                return _mapper.Map<FuncionarioDetalhesDTO>(funcionario);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao buscar detalhes do funcionário");
            }
        }

        public async Task<FuncionarioReadDTO?> BuscarFuncionarioPorId(long id)
        {
            try
            {
                if (id <= 0) throw new Exception("ID inválido");
                if (!await _repository.ExistsAsync(id)) throw new Exception("Funcionário não existe");
                var funcionario = await _repository.GetByIdAsync(id);
                return _mapper.Map<FuncionarioReadDTO>(funcionario);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao buscar funcionário");
            }
        }

        
    }
}