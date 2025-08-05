
using AutoMapper;
using Rh_Backend.DTO;
using Rh_Backend.Exceptions;
using Rh_Backend.Models;
using Rh_Backend.Repository.Interfaces;
using Rh_Backend.Services.Interfaces;

namespace Rh_Backend.Services
{
    public class ContratoService : IContratoService
    {
        private readonly IContratoRepository _contratoRepository;
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly ICargoRepository _cargoRepository;
        private readonly IMapper _mapper;
        public ContratoService(IContratoRepository contratoRepository, IFuncionarioRepository funcionarioRepository, ICargoRepository cargoRepository ,IMapper mapper)
        {
            _contratoRepository = contratoRepository;
            _funcionarioRepository = funcionarioRepository;
            _cargoRepository = cargoRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ContratoDTO>> ListarTodosContratos()
        {
            try
            {
                var contratos = await _contratoRepository.GetAllAsync() ?? throw new NotFoundException("Nenhum contrato encontrado.");
                return _mapper.Map<IEnumerable<ContratoDTO>>(contratos);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<ContratoDTO?> BuscarContratoPorId(long idCargo, long idFuncionario)
        {
            try
            {
                if (idCargo <= 0 || idFuncionario <= 0) throw new BadRequestException("ID do contrato deve ser maior que zero.");
                if (!await _contratoRepository.ExistsAsync(idFuncionario, idCargo)) throw new NotFoundException("Contrato não encontrado.");
                var contrato = await _contratoRepository.GetByIdAsync(idCargo, idFuncionario); 
                return _mapper.Map<ContratoDTO>(contrato);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }




        public async Task<bool> ExistsPorId(long idCargo, long idFuncionario)
        {
            try
            {
                if (idCargo <= 0 || idFuncionario <= 0) throw new BadRequestException("ID inválido.");
                var contrato = await _contratoRepository.ExistsAsync(idCargo, idFuncionario);
                if(!contrato) throw new NotFoundException("Contrato não encontrado.");
                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<ContratoDTO> CriarContrato(ContratoDTO contrato)
        {
            try
            {
                if (contrato == null) throw new BadRequestException("Contrato não pode ser nulo.");
                if (contrato.IdCargo <= 0 || contrato.IdFuncionario <= 0) throw new BadRequestException("IDs do cargo e funcionário devem ser maiores que zero.");
                if (!await _funcionarioRepository.ExistsAsync(contrato.IdFuncionario)) throw new BadRequestException("Funcionário não existe");
                if (!await _cargoRepository.ExistsAsync(contrato.IdCargo)) throw new BadRequestException("Cargo não existe");
                var contratoModel = _mapper.Map<ContratoModel>(contrato);
                var createdContrato = await _contratoRepository.CreateAsync(contratoModel) ?? throw new Exception("Erro ao criar contrato.");
                return _mapper.Map<ContratoDTO>(createdContrato);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<ContratoDTO> AtualizarContrato(long idFuncionario, long idCargoAntigo, long idCargoNovo)
        {
            try
            {
                if (idFuncionario <= 0 || idCargoAntigo <= 0 || idCargoNovo <= 0) throw new BadRequestException("IDs devem ser maiores que zero.");
                if (!await _contratoRepository.ExistsAsync(idFuncionario, idCargoAntigo)) throw new NotFoundException("Contrato antigo não existe");
                if (!await _funcionarioRepository.ExistsAsync(idFuncionario)) throw new BadRequestException("Funcionário não existe");
                if (!await _cargoRepository.ExistsAsync(idCargoAntigo)) throw new BadRequestException("Cargo antigo não existe");
                if (!await _cargoRepository.ExistsAsync(idCargoNovo)) throw new BadRequestException("Cargo novo não existe");
                var contrato = await _contratoRepository.UpdateAsync(idFuncionario, idCargoAntigo, idCargoNovo) ?? throw new Exception("Erro ao atualizar contrato.");
                return _mapper.Map<ContratoDTO>(contrato);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<bool> DeletarContrato(long idCargo, long idFuncionario)
        {
            try
            {
                if (idCargo <= 0 || idFuncionario <= 0) throw new BadRequestException("ID do contrato deve ser maior que zero.");
                if (!await _contratoRepository.ExistsAsync(idFuncionario, idCargo)) throw new NotFoundException("Contrato não existe");
                var deleted = await _contratoRepository.DeleteAsync(idCargo, idFuncionario);
                if (!deleted) throw new Exception("Erro ao deletar contrato.");
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