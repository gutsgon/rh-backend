
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Rh_Backend.DTO;
using Rh_Backend.Exceptions;
using Rh_Backend.Models;
using Rh_Backend.Repository.Interfaces;
using Rh_Backend.Services.Interfaces;

namespace Rh_Backend.Services
{
    public class FeriasService : IFeriasService
    {
        private readonly IFeriasRepository _feriasRepository;
        private readonly IMapper _mapper;
        private readonly IFuncionarioRepository _funcionarioRepository;

        public FeriasService(IFeriasRepository feriasRepository, IMapper mapper, IFuncionarioRepository funcionarioRepository)
        {
            _feriasRepository = feriasRepository;
            _mapper = mapper;
            _funcionarioRepository = funcionarioRepository;
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
                throw;
            }
        }

        public async Task<FeriasReadDTO?> BuscarFeriasPorId(long id)
        {
            try
            {
                if (id <= 0) throw new BadRequestException("Id deve ser maior que 0");
                var ferias = await _feriasRepository.GetByIdAsync(id) ?? throw new NotFoundException("Registro de férias não encontrado.");
                return _mapper.Map<FeriasReadDTO>(ferias);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<FeriasReadDTO>> BuscarFeriasPorIdFuncionario(long idFuncionario)
        {
            try
            {
                if (idFuncionario <= 0) throw new BadRequestException("Id deve ser maior que 0");
                if (!await _funcionarioRepository.ExistsAsync(idFuncionario)) throw new NotFoundException("Funcionário não existe");
                var ferias = await _feriasRepository.GetByIdFuncionarioAsync(idFuncionario) ?? throw new NotFoundException("Nenhum registro de férias encontrado para o funcionário.");
                return _mapper.Map<IEnumerable<FeriasReadDTO>>(ferias);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<FeriasReadDTO>> BuscarFeriasPorData(DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                if (dataInicio > dataFim) throw new BadRequestException("A data final deve ser maior que a data de inicio");
                var ferias = await _feriasRepository.GetByDataAsync(dataInicio, dataFim) ?? throw new NotFoundException("Nenhum registro de férias encontrado nesse período.");
                return _mapper.Map<IEnumerable<FeriasReadDTO>>(ferias);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<FeriasReadDTO>> BuscarFeriasPorStatus(string status)
        {
            try
            {
                Console.WriteLine($"Status recebido: '{status}'");
                if (status.Any(char.IsDigit)) throw new BadRequestException("Apenas letras são permitidas");
                if (!(status.Equals("Em andamento") || status.Equals("Pendente") || status.Equals("Concluídas"))) throw new BadRequestException("Status inválido");
                var ferias = await _feriasRepository.GetByStatusAsync(status) ?? throw new Exception("Nenhum registro de férias encontrado.");
                return _mapper.Map<IEnumerable<FeriasReadDTO>>(ferias);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<FeriasReadDTO>> BuscarFeriasPorFuncionarioEStatus(long idFuncionario, string status)
        {
            try
            {
                if (status.Any(char.IsDigit)) throw new BadRequestException("Apenas letras são permitidas");
                if (!(status.Equals("Em andamento") || status.Equals("Pendente") || status.Equals("Concluídas"))) throw new BadRequestException("Status inválido");
                if (!await _funcionarioRepository.ExistsAsync(idFuncionario)) throw new BadRequestException("Funcionario não existe");
                if (idFuncionario <= 0) throw new BadRequestException("Id não pode ser menor que 0");
                var ferias = await _feriasRepository.GetByFuncionarioAndStatusAsync(idFuncionario, status) ?? throw new Exception("Nenhum registro de férias encontrado.");
                return _mapper.Map<IEnumerable<FeriasReadDTO>>(ferias);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<FeriasReadDTO> CriarFerias(FeriasCreateDTO ferias)
        {
            try
            {
                if (ferias == null) throw new BadRequestException("Dados de férias não podem ser nulos.");
                if (ferias.IdFuncionario <= 0) throw new BadRequestException("ID do funcionário inválido.");
                if (ferias.DataInicio >= ferias.DataFim) throw new BadRequestException("Data de início deve ser anterior à data de fim.");
                var funcionario = await _funcionarioRepository.GetByIdAsync(ferias.IdFuncionario) ?? throw new NotFoundException("Funcionário não existe.");

                var feriasModel = _mapper.Map<FeriasModel>(ferias);
                if (ferias.DataInicio > DateTime.Now) feriasModel.Status = "Pendente";
                if (DateTime.Now >= ferias.DataInicio && DateTime.Now <= ferias.DataFim) feriasModel.Status = "Em andamento";
                if (ferias.DataFim < DateTime.Now) feriasModel.Status = "Concluídas";


                var novasFerias = await _feriasRepository.CreateAsync(feriasModel);
                return _mapper.Map<FeriasReadDTO>(novasFerias);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<FeriasReadDTO> AtualizarFerias(FeriasUpdateDTO ferias)
        {
            try
            {
                if (ferias == null) throw new BadRequestException("Dados de férias não podem ser nulos.");
                if (ferias.Id <= 0) throw new BadRequestException("ID de férias inválido.");
                if (ferias.DataInicio >= ferias.DataFim) throw new BadRequestException("Data de início deve ser anterior à data de fim.");
                var funcionario = await _funcionarioRepository.GetByIdAsync(ferias.IdFuncionario) ?? throw new NotFoundException("Funcionário não existe.");

                var feriasModel = _mapper.Map<FeriasModel>(ferias);
                var novasFerias = await _feriasRepository.UpdateAsync(feriasModel);
                return _mapper.Map<FeriasReadDTO>(novasFerias);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<bool> DeletarFerias(long id)
        {
            try
            {
                if (id <= 0) throw new BadRequestException("ID de férias inválido.");
                if (!await _feriasRepository.ExistsAsync(id)) throw new NotFoundException("Registro de férias não encontrado.");
                return await _feriasRepository.DeleteAsync(id);
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
                if (id <= 0) throw new BadRequestException("ID de férias inválido.");
                var ferias = await _feriasRepository.ExistsAsync(id);
                if (!ferias) throw new NotFoundException("Ferias não encontrada");
                return ferias;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }  
    }
}