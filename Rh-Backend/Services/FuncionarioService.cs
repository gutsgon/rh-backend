using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Rh_Backend.DTO;
using Rh_Backend.Exceptions;
using Rh_Backend.Models;
using Rh_Backend.Repository.Interfaces;
using Rh_Backend.Services.Interfaces;

namespace Rh_Backend.Services
{
    public class FuncionarioService : IFuncionarioService
    {
        private readonly IFuncionarioRepository _repository;
        private readonly ICargoRepository _cargoRepository;
        private readonly IContratoRepository _contratoRepository;
        private readonly IFeriasRepository _feriasRepository;
        private readonly IHistoricoAlteracaoRepository _historicoAlteracaoRepository;
        private readonly IMapper _mapper;
        public FuncionarioService(IFuncionarioRepository repository, ICargoRepository cargoRepository, IContratoRepository contratoRepository, IFeriasRepository feriasRepository, IHistoricoAlteracaoRepository historicoAlteracaoRepository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
            _cargoRepository = cargoRepository;
            _contratoRepository = contratoRepository;
            _feriasRepository = feriasRepository;
            _historicoAlteracaoRepository = historicoAlteracaoRepository;
        }

        public async Task<IEnumerable<FuncionarioReadDTO>> ListarTodosFuncionarios()
        {
            try
            {
                var funcionarios = await _repository.GetAllAsync() ?? throw new NotFoundException("Nenhum funcionário encontrado");
                var funcionariosDTO = _mapper.Map<IEnumerable<FuncionarioReadDTO>>(funcionarios);
                return funcionariosDTO;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<FuncionarioReadDTO>> BuscarFuncionarios(FuncionarioCreateDTO funcionario)
        {
            try
            {
                if (funcionario.DataAdmissao > DateTime.Now) throw new BadRequestException("Data inválida");
                if (funcionario.Nome.Any(char.IsDigit)) throw new BadRequestException("Nome inválido");
                if (funcionario.Salario < 0) throw new BadRequestException("Salário inválido");
                var funcionarios = await _repository.SearchAsync(funcionario.DataAdmissao, funcionario.Nome, funcionario.Salario, funcionario.Status);
                return _mapper.Map<IEnumerable<FuncionarioReadDTO>>(funcionarios);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<FuncionarioReadDTO>> BuscarFuncionariosPorCargo(string cargo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cargo)) throw new BadRequestException("Cargo inválido");
                if (!await _cargoRepository.ExistsByNomeAsync(cargo)) throw new NotFoundException("Cargo não existe");
                var funcionarios = await _repository.GetByCargoAsync(cargo);
                return _mapper.Map<IEnumerable<FuncionarioReadDTO>>(funcionarios);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<FuncionarioDetalhesDTO> BuscarFuncionarioDetalhado(long id)
        {
            try
            {
                if (id <= 0) throw new Exception("ID inválido");
                if (!await _repository.ExistsAsync(id)) throw new BadRequestException("Funcionário não existe");
                var funcionario = await _repository.GetWithDetailsAsync(id) ?? throw new NotFoundException("Funcionário não encontrado");
                return _mapper.Map<FuncionarioDetalhesDTO>(funcionario);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<FuncionarioReadDTO?> BuscarFuncionarioPorId(long id)
        {
            try
            {
                if (id <= 0) throw new Exception("ID inválido");
                if (!await _repository.ExistsAsync(id)) throw new BadRequestException("Funcionário não existe");
                var funcionario = await _repository.GetByIdAsync(id);
                return _mapper.Map<FuncionarioReadDTO>(funcionario);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<FuncionarioComCargoDTO>> BuscarFuncionariosComCargo()
        {
            try
            {
                return await _repository.GetFuncionariosWithCargo() ?? throw new NotFoundException("Nenhum funcionário encontrado");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<FuncionarioComCargoReadDTO> CriarFuncionario(FuncionarioComCargoCreateDTO funcionario)
        {
            try
            {
                if (funcionario == null) throw new BadRequestException("Funcionário não pode ser nulo");
                if (string.IsNullOrWhiteSpace(funcionario.Nome)) throw new BadRequestException("Nome inválido");
                if (funcionario.Salario < 0) throw new BadRequestException("Salário inválido");
                if (funcionario.DataAdmissao > DateTime.Now) throw new BadRequestException("Data de admissão inválida");
                if (funcionario.Cargo == null || funcionario.Cargo == new CargoCreateDTO()) throw new BadRequestException("Cargo inválido");
                if (!await _cargoRepository.ExistsByNomeAsync(funcionario.Cargo.Nome)) throw new BadRequestException("Cargo não existe");

                var cargoModel = await _cargoRepository.GetByNomeAsync(funcionario.Cargo.Nome) ?? throw new BadRequestException("Cargo não existe");
                var funcionarioModel = _mapper.Map<FuncionarioModel>(funcionario);
                var novoFuncionario = await _repository.CreateAsync(funcionarioModel) ?? throw new BadRequestException("Erro ao criar funcionário");
                var funcionarioComCargo = new FuncionarioComCargoReadDTO()
                {
                    Id = novoFuncionario.Id,
                    Nome = novoFuncionario.Nome,
                    DataAdmissao = novoFuncionario.DataAdmissao,
                    Salario = novoFuncionario.Salario,
                    Status = novoFuncionario.Status,
                    Cargo = funcionario.Cargo
                };

                var contratoModel = new ContratoModel
                {
                    IdCargo = cargoModel.Id,
                    IdFuncionario = funcionarioModel.Id
                };

                var novoContrato = await _contratoRepository.CreateAsync(contratoModel) ?? throw new Exception("Erro ao criar contrato");
                return funcionarioComCargo;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<FuncionarioMediaSalarioDTO> CalcularMediaSalario()
        {
            try
            {
                var funcionarios = await _repository.GetAllAsync() ?? throw new NotFoundException("Nenhum funcionário encontrado");
                if (!funcionarios.Any()) throw new Exception("Nenhum funcionário cadastrado");

                var salarioMedio = funcionarios.Average(f => f.Salario);
                return new FuncionarioMediaSalarioDTO { salarioMedio = salarioMedio };
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<FuncionarioComCargoReadDTO> AtualizarFuncionario(FuncionarioComCargoUpdateDTO funcionario)
        {
            try
            {
                if (!await _repository.ExistsAsync(funcionario.Id)) throw new NotFoundException("Funcionário não existe");
                if (string.IsNullOrWhiteSpace(funcionario.Nome)) throw new BadRequestException("Nome inválido");
                if (funcionario.Salario < 0) throw new BadRequestException("Salário inválido");
                if (funcionario.DataAdmissao > DateTime.Now) throw new BadRequestException("Data de admissão inválida");
                if (funcionario.CargoAntigo == null || funcionario.CargoAntigo == new CargoCreateDTO()) throw new NotFoundException("Cargo inválido");
                if (!await _cargoRepository.ExistsByNomeAsync(funcionario.CargoAntigo.Nome)) throw new NotFoundException("Cargo não existe");
                if (funcionario.CargoNovo == null || funcionario.CargoNovo == new CargoCreateDTO()) throw new NotFoundException("Novo cargo inválido");
                if (!await _cargoRepository.ExistsByNomeAsync(funcionario.CargoNovo.Nome)) throw new NotFoundException("Novo cargo não existe");


                var funcionarioModel = await _repository.GetByIdAsync(funcionario.Id) ?? throw new NotFoundException("Funcionário não encontrado");
                var feriasModel = await _feriasRepository.GetByFuncionarioAndStatusAsync(funcionario.Id, "Em Andamento");
                if (!feriasModel.Any()) throw new BadRequestException("Não foi possível alterar historico do funcionario, funcionario sem ferias");
                var cargoAntigoModel = await _cargoRepository.GetByNomeAsync(funcionario.CargoAntigo.Nome) ?? throw new NotFoundException("Cargo não encontrado");
                var cargoNovoModel = await _cargoRepository.GetByNomeAsync(funcionario.CargoNovo.Nome) ?? throw new NotFoundException("Cargo não encontrado");

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
                    await _historicoAlteracaoRepository.CreateAsync(_mapper.Map<HistoricoAlteracaoModel>(historico));
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
                    await _historicoAlteracaoRepository.CreateAsync(_mapper.Map<HistoricoAlteracaoModel>(historico));
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
                    await _historicoAlteracaoRepository.CreateAsync(_mapper.Map<HistoricoAlteracaoModel>(historico));
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
                    await _historicoAlteracaoRepository.CreateAsync(_mapper.Map<HistoricoAlteracaoModel>(historico));
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
                    await _historicoAlteracaoRepository.CreateAsync(_mapper.Map<HistoricoAlteracaoModel>(historico));
                    await _contratoRepository.UpdateAsync(funcionario.Id, cargoAntigoModel.Id, cargoNovoModel.Id);
                }

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
                await _repository.UpdateAsync(funcionarioModel);

                return funcionarioComCargo;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<bool> DeletarFuncionario(long id)
        {
            try
            {
                if (id <= 0) throw new Exception("ID inválido");
                if (!await _repository.ExistsAsync(id)) throw new NotFoundException("Funcionário não existe");
                if (await _contratoRepository.ExistsFuncionarioAsync(id)) throw new BadRequestException("Contrato com esse funcionario ainda existe");
                await _repository.DeleteAsync(id);
                return true;
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
                if (id <= 0) throw new BadRequestException("Id deve ser maior que 0");
                return await _repository.ExistsAsync(id);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }
        
        
    }
}