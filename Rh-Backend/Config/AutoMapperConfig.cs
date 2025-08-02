using AutoMapper;
using Rh_Backend.DTO;
using Rh_Backend.Models;

namespace Rh_Backend.Config
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            // DTO <-> Model dos Cargos
            CreateMap<CargoCreateDTO, CargoModel>().ReverseMap();
            CreateMap<CargoReadDTO, CargoModel>().ReverseMap();
            CreateMap<CargoUpdateDTO, CargoModel>().ReverseMap();

            // DTO <-> Model dos Contratos
            CreateMap<ContratoDTO, ContratoModel>().ReverseMap();

            // DTO <-> Model das Ferias
            CreateMap<FeriasCreateDTO, FeriasModel>().ReverseMap();
            CreateMap<FeriasReadDTO, FeriasModel>().ReverseMap();
            CreateMap<FeriasUpdateDTO, FeriasModel>().ReverseMap();

            // DTO <-> Model dos Funcionarios
            CreateMap<FuncionarioCreateDTO, FuncionarioModel>().ReverseMap();
            CreateMap<FuncionarioReadDTO, FuncionarioModel>().ReverseMap();
            CreateMap<FuncionarioUpdateDTO, FuncionarioModel>().ReverseMap();

            // DTO <-> Model do Historico de Alteração
            CreateMap<HistoricoAlteracaoCreateDTO, HistoricoAlteracaoModel>().ReverseMap();
            CreateMap<HistoricoAlteracaoReadDTO, HistoricoAlteracaoModel>().ReverseMap();
            CreateMap<HistoricoAlteracaoUpdateDTO, HistoricoAlteracaoModel>().ReverseMap();


        }
    }
}