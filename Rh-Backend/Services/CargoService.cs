using AutoMapper;
using Rh_Backend.DTO;
using Rh_Backend.Models;
using Rh_Backend.Repository.Interfaces;
using Rh_Backend.Services.Interfaces;

namespace Rh_Backend.Services
{
    public class CargoService : ICargoService
    {
        private readonly ICargoRepository _cargoRepository;
        private readonly IMapper _mapper;

        public CargoService(ICargoRepository cargoRepository, IMapper mapper)
        {
            _cargoRepository = cargoRepository;
            _mapper = mapper;
        }

        public async Task<bool> ExistsPorNome(string nome)
        {
            try
            {
                if (string.IsNullOrEmpty(nome)) throw new Exception("Nome do cargo não pode ser nulo ou vazio.");
                var cargo = await _cargoRepository.GetByNomeAsync(nome) ?? throw new Exception("Cargo não encontrado.");
                if (cargo == null) return false;
                return true;

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao buscar cargo");
            }
        }

        public async Task<bool> Exists(long id)
        {
            try
            {
                if (id <= 0) throw new Exception("ID do cargo deve ser maior que zero.");
                var cargo = await _cargoRepository.GetByIdAsync(id) ?? throw new Exception("Cargo não encontrado.");
                if (cargo == null) return false;
                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao verificar existência do cargo");
            }
        }

        public async Task<CargoReadDTO?> BuscarCargoPorNome(string nome)
        {
            try
            {
                if (string.IsNullOrEmpty(nome)) throw new Exception("Nome do cargo não pode ser nulo ou vazio.");
                var cargo = await _cargoRepository.GetByNomeAsync(nome) ?? throw new Exception("Cargo não encontrado.");
                return _mapper.Map<CargoReadDTO>(cargo);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao buscar cargo");
            }
        }

        public async Task<CargoReadDTO?> BuscarCargoPorId(long id)
        {
            try
            {
                if (id <= 0) throw new Exception("ID do cargo deve ser maior que zero.");
                var cargo = await _cargoRepository.GetByIdAsync(id) ?? throw new Exception("Cargo não encontrado.");
                return _mapper.Map<CargoReadDTO>(cargo);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao buscar cargo");
            }
        }

        public async Task<IEnumerable<CargoReadDTO>> ListarTodosCargos()
        {
            try
            {
                var cargos = await _cargoRepository.GetAllAsync() ?? throw new Exception("Nenhum cargo encontrado.");
                return _mapper.Map<IEnumerable<CargoReadDTO>>(cargos);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao listar cargos");
            }
        }

        public async Task<CargoReadDTO> CriarCargo(CargoCreateDTO cargo)
        {
            try
            {
                if (cargo == null) throw new Exception("Dados do cargo não podem ser nulos.");
                if (string.IsNullOrEmpty(cargo.Nome)) throw new Exception("Nome do cargo não pode ser nulo ou vazio.");
                if (cargo.Nome.Length > 50) throw new Exception("Nome do cargo não pode ter mais de 50 caracteres.");
                if (await ExistsPorNome(cargo.Nome)) throw new Exception("Cargo com este nome já existe.");
                return _mapper.Map<CargoReadDTO>(cargo);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao criar cargo");
            }
        }

        public async Task<CargoReadDTO> AtualizarCargo(CargoUpdateDTO cargo)
        {
            try
            {
                if (cargo.Id <= 0) throw new Exception("IDs devem ser maiores que zero.");
                if (string.IsNullOrEmpty(cargo.Nome)) throw new Exception("Nome do cargo não pode ser nulo ou vazio.");
                if (cargo.Nome.Length > 50) throw new Exception("Nome do cargo não pode ter mais de 50 caracteres.");
                if (!await Exists(cargo.Id)) throw new Exception("Cargo não encontrado.");
                if (await ExistsPorNome(cargo.Nome)) throw new Exception("Cargo com este nome já existe.");
                var cargoModel = _mapper.Map<CargoModel>(cargo);
                var cargoNovo = await _cargoRepository.UpdateAsync(cargoModel) ?? throw new Exception("Erro ao atualizar cargo.");
                return _mapper.Map<CargoReadDTO>(cargoNovo);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao atualizar cargo");
            }
        }
        
        public async Task<bool> DeletarCargo(long id)
        {
            try
            {
                if (id <= 0) throw new Exception("ID do cargo deve ser maior que zero.");
                if (!await Exists(id)) throw new Exception("Cargo não encontrado.");
                return await _cargoRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao deletar cargo");
            }
        }
    }
}