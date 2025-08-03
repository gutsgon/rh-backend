
using AutoMapper;
using Rh_Backend.DTO;
using Rh_Backend.Models;
using Rh_Backend.Repository.Interfaces;
using Rh_Backend.Services.Interfaces;

namespace Rh_Backend.Services
{
    public class ContratoService : IContratoService
    {
        private readonly IContratoRepository _contratoRepository;
        private readonly IMapper _mapper;
        public ContratoService(IContratoRepository contratoRepository, IMapper mapper)
        {
            _contratoRepository = contratoRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ContratoDTO>> ListarTodosContratos()
        {
            try
            {
                var contratos = await _contratoRepository.GetAllAsync() ?? throw new Exception("Nenhum contrato encontrado.");
                return _mapper.Map<IEnumerable<ContratoDTO>>(contratos);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao listar contratos");
            }
        }

        public async Task<ContratoDTO?> BuscarContratoPorId(long idCargo, long idFuncionario)
        {
            try
            {
                if (idCargo <= 0 | idFuncionario <= 0) throw new Exception("ID do contrato deve ser maior que zero.");
                var contrato = await _contratoRepository.GetByIdAsync(idCargo, idFuncionario) ?? throw new Exception("Contrato não encontrado.");
                return _mapper.Map<ContratoDTO>(contrato);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao buscar contrato");
            }
        }




        public async Task<bool> ExistsPorId(long idCargo, long idFuncionario)
        {
            try
            {
                if (idCargo <= 0 | idFuncionario <= 0) throw new Exception("ID inválido.");
                var contrato = await _contratoRepository.GetByIdAsync(idCargo, idFuncionario) ?? throw new Exception("Contrato não encontrado.");
                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao buscar contrato");
            }
        }

        public async Task<ContratoDTO> CriarContrato(ContratoDTO contrato)
        {
            try
            {
                if (contrato == null) throw new Exception("Contrato não pode ser nulo.");
                if (contrato.IdCargo <= 0 || contrato.IdFuncionario <= 0) throw new Exception("IDs do cargo e funcionário devem ser maiores que zero.");
                var contratoModel = _mapper.Map<ContratoModel>(contrato);
                var createdContrato = await _contratoRepository.CreateAsync(contratoModel) ?? throw new Exception("Erro ao criar contrato.");
                return _mapper.Map<ContratoDTO>(createdContrato);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao criar contrato");
            }
        }

        public async Task<ContratoDTO> AtualizarContrato(long idFuncionario, long idCargoAntigo, long idCargoNovo)
        {
            try
            {
                if (idFuncionario <= 0 || idCargoAntigo <= 0 || idCargoNovo <= 0) throw new Exception("IDs devem ser maiores que zero.");
                var contrato = await _contratoRepository.UpdateAsync(idFuncionario, idCargoAntigo, idCargoNovo) ?? throw new Exception("Erro ao atualizar contrato.");
                return _mapper.Map<ContratoDTO>(contrato);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao atualizar contrato");
            }
        }

        public async Task<bool> DeletarContrato(long idCargo, long idFuncionario)
        {
            try
            {
                if (idCargo <= 0 | idFuncionario <= 0) throw new Exception("ID do contrato deve ser maior que zero.");
                var deleted = await _contratoRepository.DeleteAsync(idCargo, idFuncionario);
                if (!deleted) throw new Exception("Erro ao deletar contrato.");
                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Erro ao deletar contrato");
            }
        }
    }
}