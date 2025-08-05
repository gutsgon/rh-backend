using Xunit;
using Moq;
using Rh_Backend.Services;
using Rh_Backend.Repository.Interfaces;
using Rh_Backend.DTO;
using AutoMapper;
using Rh_Backend.Models;

public class FuncionarioServiceTests
{
    private FuncionarioService GetService(
        Mock<IMapper> mockMapper,
        Mock<IFuncionarioRepository> mockRepo,
        Mock<ICargoRepository> mockCargoRepository,
        Mock<IContratoRepository> mockContratoRepository,
        Mock<IFeriasRepository> mockFeriasRepository,
        Mock<IHistoricoAlteracaoRepository> mockHistoricoAlteracaoRepository)
    {
        return new FuncionarioService(
            mockRepo.Object,
            mockCargoRepository.Object,
            mockContratoRepository.Object,
            mockFeriasRepository.Object,
            mockHistoricoAlteracaoRepository.Object,
            mockMapper.Object
        );
    }

    [Fact]
    public async Task CriarFuncionario_DeveRetornarFuncionarioReadDTO_QuandoSucesso()
    {
        var mockRepo = new Mock<IFuncionarioRepository>();
        var mockMapper = new Mock<IMapper>();
        var mockCargoRepository = new Mock<ICargoRepository>();
        var mockContratoRepository =new Mock<IContratoRepository>();
        var mockFeriasRepository = new Mock<IFeriasRepository>();
        var mockHistoricoAlteracaoRepository = new Mock<IHistoricoAlteracaoRepository>();

        var createDTO = new FuncionarioComCargoCreateDTO { Cargo = new CargoCreateDTO { Nome = "Analista"},Nome = "João", DataAdmissao = new DateTime().AddDays(-1), Salario = 1500, Status = true };
        var model = new FuncionarioModel { Id = 1, Nome = "João" };
        var readDTO = new FuncionarioReadDTO { Id = 1, Nome = "João" };

        mockCargoRepository.Setup(r => r.ExistsByNomeAsync(It.IsAny<string>())).ReturnsAsync(true);
        mockCargoRepository.Setup(r => r.GetByNomeAsync(It.IsAny<string>())).ReturnsAsync(new CargoModel { Id = 1, Nome = "Analista" });
        mockRepo.Setup(r => r.ExistsAsync(It.IsAny<long>())).ReturnsAsync(true);
        mockFeriasRepository.Setup(r => r.GetByFuncionarioAndStatusAsync(It.IsAny<long>(), It.IsAny<string>())).ReturnsAsync(new List<FeriasModel> { new FeriasModel { Id = 1 } });
        mockContratoRepository.Setup(r => r.ExistsFuncionarioAsync(It.IsAny<long>())).ReturnsAsync(false);
        mockHistoricoAlteracaoRepository.Setup(r => r.CreateAsync(It.IsAny<HistoricoAlteracaoModel>())).ReturnsAsync(new HistoricoAlteracaoModel { Id = 1 });

        mockRepo.Setup(r => r.CreateAsync(model)).ReturnsAsync(model);
        mockMapper.Setup(m => m.Map<FuncionarioModel>(createDTO)).Returns(model);
        mockMapper.Setup(m => m.Map<FuncionarioReadDTO>(model)).Returns(readDTO);

        var service = GetService(mockMapper, mockRepo, mockCargoRepository, mockContratoRepository, mockFeriasRepository, mockHistoricoAlteracaoRepository);

        var result = await service.CriarFuncionario(createDTO);

        Assert.NotNull(result);
        Assert.Equal("João", result.Nome);
    }

    [Fact]
    public async Task BuscarFuncionarioPorId_DeveRetornarFuncionarioReadDTO_QuandoEncontrado()
    {
        var mockRepo = new Mock<IFuncionarioRepository>();
        var mockMapper = new Mock<IMapper>();
        var mockCargoRepository = new Mock<ICargoRepository>();
        var mockContratoRepository =new Mock<IContratoRepository>();
        var mockFeriasRepository = new Mock<IFeriasRepository>();
        var mockHistoricoAlteracaoRepository = new Mock<IHistoricoAlteracaoRepository>();

        var model = new FuncionarioModel { Id = 1, Nome = "João" };
        var readDTO = new FuncionarioReadDTO { Id = 1, Nome = "João" };

        mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(model);
        mockMapper.Setup(m => m.Map<FuncionarioReadDTO>(model)).Returns(readDTO);

        var service = GetService(mockMapper, mockRepo, mockCargoRepository, mockContratoRepository, mockFeriasRepository, mockHistoricoAlteracaoRepository);

        var result = await service.BuscarFuncionarioPorId(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task AtualizarFuncionario_DeveRetornarFuncionarioReadDTO_QuandoSucesso()
    {
        var mockRepo = new Mock<IFuncionarioRepository>();
        var mockMapper = new Mock<IMapper>();
        var mockCargoRepository = new Mock<ICargoRepository>();
        var mockContratoRepository =new Mock<IContratoRepository>();
        var mockFeriasRepository = new Mock<IFeriasRepository>();
        var mockHistoricoAlteracaoRepository = new Mock<IHistoricoAlteracaoRepository>();

        var updateDTO = new FuncionarioComCargoUpdateDTO { Nome = "João Silva", Id = 1, Status = true, CargoAntigo = new CargoCreateDTO { Nome = "Analista"}, CargoNovo = new CargoCreateDTO { Nome = "Analista Sênior"}, DataAdmissao = new DateTime().AddDays(-1), Salario = 1500};
        var model = new FuncionarioModel { Nome = "João Silva", Id = 1, Status = true, DataAdmissao = new DateTime().AddDays(-1), Salario = 1500};
        var readDTO = new FuncionarioReadDTO { Nome = "João Silva", Id = 1, Status = true, DataAdmissao = new DateTime().AddDays(-1), Salario = 1500};

        mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(model);
        mockRepo.Setup(r => r.UpdateAsync(It.IsAny<FuncionarioModel>())).ReturnsAsync(model);
        mockMapper.Setup(m => m.Map<FuncionarioModel>(updateDTO)).Returns(model);
        mockMapper.Setup(m => m.Map<FuncionarioReadDTO>(model)).Returns(readDTO);

        var service = GetService(mockMapper, mockRepo, mockCargoRepository, mockContratoRepository, mockFeriasRepository, mockHistoricoAlteracaoRepository);

        var result = await service.AtualizarFuncionario(updateDTO);

        Assert.NotNull(result);
        Assert.Equal("João Silva", result.Nome);
    }

    [Fact]
    public async Task DeletarFuncionario_DeveRetornarTrue_QuandoSucesso()
    {
        var mockRepo = new Mock<IFuncionarioRepository>();
        var mockMapper = new Mock<IMapper>();
        var mockCargoRepository = new Mock<ICargoRepository>();
        var mockContratoRepository =new Mock<IContratoRepository>();
        var mockFeriasRepository = new Mock<IFeriasRepository>();
        var mockHistoricoAlteracaoRepository = new Mock<IHistoricoAlteracaoRepository>();

        mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        var service = GetService(mockMapper, mockRepo, mockCargoRepository, mockContratoRepository, mockFeriasRepository, mockHistoricoAlteracaoRepository);

        var result = await service.DeletarFuncionario(1);

        Assert.True(result);
    }

    [Fact]
    public async Task ListarTodosFuncionarios_DeveRetornarListaDeFuncionarioReadDTO()
    {
        var mockRepo = new Mock<IFuncionarioRepository>();
        var mockMapper = new Mock<IMapper>();
        var mockCargoRepository = new Mock<ICargoRepository>();
        var mockContratoRepository =new Mock<IContratoRepository>();
        var mockFeriasRepository = new Mock<IFeriasRepository>();
        var mockHistoricoAlteracaoRepository = new Mock<IHistoricoAlteracaoRepository>();

        var models = new List<FuncionarioModel>
        {
            new FuncionarioModel { Id = 1, Nome = "João" },
            new FuncionarioModel { Id = 2, Nome = "Maria" }
        };
        var dtos = new List<FuncionarioReadDTO>
        {
            new FuncionarioReadDTO { Id = 1, Nome = "João" },
            new FuncionarioReadDTO { Id = 2, Nome = "Maria" }
        };

        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(models);
        mockMapper.Setup(m => m.Map<IEnumerable<FuncionarioReadDTO>>(models)).Returns(dtos);

        var service = GetService(mockMapper, mockRepo, mockCargoRepository, mockContratoRepository, mockFeriasRepository, mockHistoricoAlteracaoRepository);

        var result = await service.ListarTodosFuncionarios();

        Assert.NotNull(result);
        Assert.Equal(2, ((List<FuncionarioReadDTO>)result).Count);
    }
}