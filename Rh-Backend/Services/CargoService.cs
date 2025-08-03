using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Rh_Backend.DTO;
using Rh_Backend.Exceptions;
using Rh_Backend.Models;
using Rh_Backend.Repository.Interfaces;
using Rh_Backend.Services.Interfaces;

namespace Rh_Backend.Services
{
    public class CargoService : ICargoService
    {
        private readonly ICargoRepository _cargoRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CargoService> _logger;

        public CargoService(ICargoRepository cargoRepository, IMapper mapper, ILogger<CargoService> logger)
        {
            _cargoRepository = cargoRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> ExistsPorNome(string nome)
        {
            try
            {
                if (string.IsNullOrEmpty(nome)) throw new BadRequestException("Nome do cargo não pode ser nulo ou vazio.");
                if (nome.Any(char.IsDigit)) throw new BadRequestException("Apenas letras são permitidas");
                var cargo = await _cargoRepository.ExistsByNomeAsync(nome);
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
                if (id <= 0) throw new BadRequestException("ID do cargo deve ser maior que zero.");
                var cargo = await _cargoRepository.GetByIdAsync(id) ?? throw new NotFoundException("Cargo não encontrado.");
                if (cargo == null) return false;
                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<CargoReadDTO?> BuscarCargoPorNome(string nome)
        {
            try
            {
                if (string.IsNullOrEmpty(nome)) throw new BadRequestException("Nome do cargo não pode ser nulo ou vazio.");
                if (nome.Any(char.IsDigit)) throw new BadRequestException("Apenas letras são permitidas");
                var cargo = await _cargoRepository.GetByNomeAsync(nome) ?? throw new NotFoundException("Cargo não encontrado.");
                return _mapper.Map<CargoReadDTO>(cargo);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<CargoReadDTO?> BuscarCargoPorId(long id)
        {
            try
            {
                if (id <= 0) throw new BadRequestException("ID do cargo deve ser maior que zero.");
                var cargo = await _cargoRepository.GetByIdAsync(id) ?? throw new NotFoundException("Cargo não encontrado.");
                return _mapper.Map<CargoReadDTO>(cargo);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<CargoReadDTO>> ListarTodosCargos()
        {
            try
            {
                var cargos = await _cargoRepository.GetAllAsync() ?? throw new NotFoundException("Nenhum cargo encontrado.");
                return _mapper.Map<IEnumerable<CargoReadDTO>>(cargos);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<CargoReadDTO> CriarCargo(CargoCreateDTO cargo)
        {
            try
            {
                if (cargo == null) throw new BadRequestException("Dados do cargo não podem ser nulos.");
                if (string.IsNullOrEmpty(cargo.Nome)) throw new BadRequestException("Nome do cargo não pode ser nulo ou vazio.");
                if (cargo.Nome.Length > 50) throw new BadRequestException("Nome do cargo não pode ter mais de 50 caracteres.");
                if (await ExistsPorNome(cargo.Nome)) throw new BadRequestException("Cargo com este nome já existe.");
                var teste = _mapper.Map<CargoModel>(cargo);
                var cargoModel = await _cargoRepository.CreateAsync(_mapper.Map<CargoModel>(cargo));
                return _mapper.Map<CargoReadDTO>(cargoModel);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<CargoReadDTO> AtualizarCargo(CargoUpdateDTO cargo)
        {
            try
            {
                if (cargo.Id <= 0) throw new BadRequestException("IDs devem ser maiores que zero.");
                if (string.IsNullOrEmpty(cargo.Nome)) throw new BadRequestException("Nome do cargo não pode ser nulo ou vazio.");
                if (cargo.Nome.Length > 50) throw new BadRequestException("Nome do cargo não pode ter mais de 50 caracteres.");
                if (!await Exists(cargo.Id)) throw new NotFoundException("Cargo não encontrado.");
                var cargoModel = _mapper.Map<CargoModel>(cargo);
                _logger.LogInformation(cargoModel.Nome);
                var cargoNovo = await _cargoRepository.UpdateAsync(cargoModel) ?? throw new Exception("Erro ao atualizar cargo.");
                if (cargoNovo == null) return new CargoReadDTO();
                _logger.LogInformation(cargoNovo.Nome);
                return _mapper.Map<CargoReadDTO>(cargoNovo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                //Console.Write(ex.Message);
                throw;
            }
        }
        
        public async Task<bool> DeletarCargo(long id)
        {
            try
            {
                if (id <= 0) throw new BadRequestException("ID do cargo deve ser maior que zero.");
                if (!await Exists(id)) throw new NotFoundException("Cargo não encontrado.");
                return await _cargoRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }
    }
}