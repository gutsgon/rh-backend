using AutoMapper;
using Rh_Backend.DTO;
using Rh_Backend.Models;
using Rh_Backend.Repository.Interfaces;
using Rh_Backend.Services.Interfaces;

namespace Rh_Backend.Services
{
    public class FuncionarioService : IFuncionarioService
    {
        private readonly IFuncionarioRepository _repository;

        /// <summary>
        /// MODIFICAR TODOS OS REPOSITORYS POR INTERFACES DOS SERVIÇOS
        /// </summary>
        private readonly ICargoService _cargoService;
        private readonly IContratoService _contratoService;
        private readonly IFeriasService _feriasService;
        private readonly IHistoricoAlteracaoService _historicoAlteracaoService;
        private readonly IMapper _mapper;
        public FuncionarioService(IFuncionarioRepository repository, ICargoService cargoService, IContratoService contratoService, IFeriasService feriasService, IHistoricoAlteracaoService historicoAlteracaoService, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
            _cargoService = cargoService;
            _contratoService = contratoService;
            _feriasService = feriasService;
            _historicoAlteracaoService = historicoAlteracaoService;
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
                var funcionario = await _repository.GetWithDetailsAsync(id) ?? throw new Exception("Funcionário não encontrado");
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

        public async Task<IEnumerable<FuncionarioComCargoDTO>> BuscarFuncionariosComCargo()
        {
            try
            {
                return await _repository.GetFuncionariosWithCargo() ?? throw new Exception("Nenhum funcionário encontrado");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao buscar funcionários com cargo");
            }
        }

        public async Task<FuncionarioComCargoReadDTO> CriarFuncionario(FuncionarioComCargoCreateDTO funcionario)
        {
            try
            {
                if (funcionario == null) throw new Exception("Funcionário não pode ser nulo");
                if (string.IsNullOrWhiteSpace(funcionario.Nome)) throw new Exception("Nome inválido");
                if (funcionario.Salario < 0) throw new Exception("Salário inválido");
                if (funcionario.DataAdmissao > DateTime.Now) throw new Exception("Data de admissão inválida");
                if (funcionario.Cargo == null || funcionario.Cargo == new CargoCreateDTO()) throw new Exception("Cargo inválido");
                if (!await _cargoService.ExistsPorNome(funcionario.Cargo.Nome)) throw new Exception("Cargo não existe");

                var cargoModel = _cargoService.BuscarCargoPorNome(funcionario.Cargo.Nome);
                var funcionarioModel = _mapper.Map<FuncionarioModel>(funcionario);
                var novoFuncionario = await _repository.CreateAsync(funcionarioModel) ?? throw new Exception("Erro ao criar funcionário");
                var funcionarioComCargo = new FuncionarioComCargoReadDTO()
                {
                    Id = novoFuncionario.Id,
                    Nome = novoFuncionario.Nome,
                    DataAdmissao = novoFuncionario.DataAdmissao,
                    Salario = novoFuncionario.Salario,
                    Status = novoFuncionario.Status,
                    Cargo = funcionario.Cargo
                };

                var contrato = new ContratoDTO
                {
                    IdFuncionario = novoFuncionario.Id,
                    IdCargo = cargoModel.Id
                };

                var novoContrato = await _contratoService.CriarContrato(contrato) ?? throw new Exception("Erro ao criar contrato");
                return funcionarioComCargo;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao criar funcionário");
            }
        }

        public async Task<FuncionarioMediaSalarioDTO> CalcularMediaSalario()
        {
            try
            {
                var funcionarios = await _repository.GetAllAsync() ?? throw new Exception("Nenhum funcionário encontrado");
                if (!funcionarios.Any()) throw new Exception("Nenhum funcionário cadastrado");

                var salarioMedio = funcionarios.Average(f => f.Salario);
                return new FuncionarioMediaSalarioDTO { salarioMedio = salarioMedio };
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao calcular média salarial");
            }
        }

        public async Task<FuncionarioComCargoReadDTO> AtualizarFuncionario(FuncionarioComCargoUpdateDTO funcionario)
        {
            try
            {
                if (!await _repository.ExistsAsync(funcionario.Id)) throw new Exception("Funcionário não existe");
                if (string.IsNullOrWhiteSpace(funcionario.Nome)) throw new Exception("Nome inválido");
                if (funcionario.Salario < 0) throw new Exception("Salário inválido");
                if (funcionario.DataAdmissao > DateTime.Now) throw new Exception("Data de admissão inválida");
                if (funcionario.CargoAntigo == null || funcionario.CargoAntigo == new CargoCreateDTO()) throw new Exception("Cargo inválido");
                if (!await _cargoService.ExistsPorNome(funcionario.CargoAntigo.Nome)) throw new Exception("Cargo não existe");
                if (funcionario.CargoNovo == null || funcionario.CargoNovo == new CargoCreateDTO()) throw new Exception("Novo cargo inválido");
                if (!await _cargoService.ExistsPorNome(funcionario.CargoNovo.Nome)) throw new Exception("Novo cargo não existe");


                var funcionarioModel = await _repository.GetByIdAsync(funcionario.Id) ?? throw new Exception("Funcionário não encontrado");
                var feriasModel = await _feriasService.BuscarFeriasPorFuncionarioEStatus(funcionario.Id, "Em Andamento");
                var cargoAntigoModel = await _cargoService.BuscarCargoPorNome(funcionario.CargoAntigo.Nome) ?? throw new Exception("Cargo não encontrado");
                var cargoNovoModel = await _cargoService.BuscarCargoPorNome(funcionario.CargoNovo.Nome) ?? throw new Exception("Cargo não encontrado");

                if (funcionarioModel.Nome != funcionario.Nome)
                {
                    var historico = new HistoricoAlteracaoCreateDTO
                    {
                        IdFuncionario = funcionario.Id,
                        IdFerias = feriasModel.First().Id,
                        IdCargo = cargoAntigoModel.Id,
                        CampoAlterado = "nome",
                        DataAlteracao = DateTime.Now,
                        ValorAntigo = funcionarioModel.Nome,
                        ValorNovo = funcionario.Nome

                    };
                    await _historicoAlteracaoService.CriarHistorico(_mapper.Map<HistoricoAlteracaoCreateDTO>(historico));
                    funcionarioModel.Nome = funcionario.Nome;
                }

                if (funcionarioModel.Salario != funcionario.Salario)
                {
                    var historico = new HistoricoAlteracaoCreateDTO
                    {
                        IdFuncionario = funcionario.Id,
                        IdFerias = feriasModel.First().Id,
                        IdCargo = cargoAntigoModel.Id,
                        CampoAlterado = "salario",
                        DataAlteracao = DateTime.Now,
                        ValorAntigo = funcionarioModel.Salario.ToString(),
                        ValorNovo = funcionario.Salario.ToString()
                    };
                    await _historicoAlteracaoService.CriarHistorico(_mapper.Map<HistoricoAlteracaoCreateDTO>(historico));
                    funcionarioModel.Salario = funcionario.Salario;
                }

                if (funcionarioModel.DataAdmissao != funcionario.DataAdmissao)
                {
                    var historico = new HistoricoAlteracaoCreateDTO
                    {
                        IdFuncionario = funcionario.Id,
                        IdFerias = feriasModel.First().Id,
                        IdCargo = cargoAntigoModel.Id,
                        CampoAlterado = "dataAdmissao",
                        DataAlteracao = DateTime.Now,
                        ValorAntigo = funcionarioModel.DataAdmissao.ToString("yyyy-MM-dd"),
                        ValorNovo = funcionario.DataAdmissao.ToString("yyyy-MM-dd")
                    };
                    await _historicoAlteracaoService.CriarHistorico(_mapper.Map<HistoricoAlteracaoCreateDTO>(historico));
                    funcionarioModel.DataAdmissao = funcionario.DataAdmissao;
                }

                if (funcionarioModel.Status != funcionario.Status)
                {
                    var historico = new HistoricoAlteracaoCreateDTO
                    {
                        IdFuncionario = funcionario.Id,
                        IdFerias = feriasModel.First().Id,
                        IdCargo = cargoAntigoModel.Id,
                        CampoAlterado = "status",
                        DataAlteracao = DateTime.Now,
                        ValorAntigo = funcionarioModel.Status.ToString(),
                        ValorNovo = funcionario.Status.ToString()
                    };
                    await _historicoAlteracaoService.CriarHistorico(_mapper.Map<HistoricoAlteracaoCreateDTO>(historico));
                    funcionarioModel.Status = funcionario.Status;
                }

                if (funcionario.CargoAntigo.Nome != funcionario.CargoNovo.Nome)
                {
                    var historico = new HistoricoAlteracaoCreateDTO
                    {
                        IdFuncionario = funcionario.Id,
                        IdFerias = feriasModel.First().Id,
                        IdCargo = cargoNovoModel.Id,
                        CampoAlterado = "cargo",
                        DataAlteracao = DateTime.Now,
                        ValorAntigo = funcionario.CargoAntigo.Nome,
                        ValorNovo = funcionario.CargoNovo.Nome
                    };
                    await _historicoAlteracaoService.CriarHistorico(_mapper.Map<HistoricoAlteracaoCreateDTO>(historico));
                    await _contratoService.AtualizarContrato(funcionario.Id, cargoAntigoModel.Id, cargoNovoModel.Id);
                }

                await _repository.UpdateAsync(funcionarioModel);

                var funcionarioComCargo = new FuncionarioComCargoReadDTO
                {
                    Id = funcionarioModel.Id,
                    Nome = funcionarioModel.Nome,
                    DataAdmissao = funcionarioModel.DataAdmissao,
                    Salario = funcionarioModel.Salario,
                    Status = funcionarioModel.Status,
                    Cargo = new CargoCreateDTO
                    {
                        Nome = funcionario.CargoNovo.Nome
                    }
                };

                return funcionarioComCargo;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao atualizar funcionário");
            }
        }

        public async Task<bool> DeletarFuncionario(long id)
        {
            try
            {
                if (id <= 0) throw new Exception("ID inválido");
                if (!await _repository.ExistsAsync(id)) throw new Exception("Funcionário não existe");
                await _repository.DeleteAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao deletar funcionário");
            }
        }
        
        public async Task<bool> Exists(long id)
        {
            try
            {
                return await _repository.ExistsAsync(id);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro, funcionário não existe");
            }
        }
        
        
    }
}