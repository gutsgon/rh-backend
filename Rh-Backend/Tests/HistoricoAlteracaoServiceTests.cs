using Xunit;
using Moq;
using Rh_Backend.Services;
using Rh_Backend.Repository.Interfaces;
using Rh_Backend.DTO;
using AutoMapper;
using Rh_Backend.Models;

public class HistoricoAlteracaoServiceTests
{
    private HistoricoAlteracaoService GetService(
        Mock<IHistoricoAlteracaoRepository> mockRepo,
        Mock<IFuncionarioRepository> mockFuncionarioRepository,
        Mock<ICargoRepository> mockCargoRepository,
        Mock<IFeriasRepository> mockFeriasRepository,
        Mock<IMapper> mockMapper)
    {
        return new HistoricoAlteracaoService(
            mockRepo.Object,
            mockFuncionarioRepository.Object,
            mockCargoRepository.Object,
            mockFeriasRepository.Object,
            mockMapper.Object
        );
    }

    [Fact]
    public async Task CriarHistorico_DeveRetornarHistoricoReadDTO_QuandoSucesso()
    {
        var mockRepo = new Mock<IHistoricoAlteracaoRepository>();
        var mockMapper = new Mock<IMapper>();
        var mockFuncionarioRepository = new Mock<IFuncionarioRepository>();
        var mockCargoRepository = new Mock<ICargoRepository>();
        var mockFeriasRepository = new Mock<IFeriasRepository>();

        var createDTO = new HistoricoAlteracaoCreateDTO { DataAlteracao = DateTime.Now, ValorAntigo = "Gustavo", ValorNovo = "Vinícius", CampoAlterado = "Nome", IdFuncionario = 1, IdCargo = 1, IdFerias = 1 };
        var model = new HistoricoAlteracaoModel { DataAlteracao = DateTime.Now, ValorAntigo = "Gustavo", ValorNovo = "Vinícius", CampoAlterado = "Nome", IdFuncionario = 1, IdCargo = 1, IdFerias = 1 };
        var readDTO = new HistoricoAlteracaoReadDTO { DataAlteracao = DateTime.Now, ValorAntigo = "Gustavo", ValorNovo = "Vinícius", CampoAlterado = "Nome", IdFuncionario = 1, IdCargo = 1, IdFerias = 1 };

        // Mocks para existência dos IDs
        mockFuncionarioRepository.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);
        mockCargoRepository.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);
        mockFeriasRepository.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);

        mockRepo.Setup(r => r.CreateAsync(It.IsAny<HistoricoAlteracaoModel>())).ReturnsAsync(model);
        mockMapper.Setup(m => m.Map<HistoricoAlteracaoModel>(createDTO)).Returns(model);
        mockMapper.Setup(m => m.Map<HistoricoAlteracaoReadDTO>(model)).Returns(readDTO);

        var service = GetService(mockRepo, mockFuncionarioRepository, mockCargoRepository, mockFeriasRepository, mockMapper);

        var result = await service.CriarHistorico(createDTO);

        Assert.NotNull(result);
        Assert.Equal("Nome", result.CampoAlterado);
    }

    [Fact]
    public async Task BuscarHistoricoPorId_DeveRetornarHistoricoReadDTO_QuandoEncontrado()
    {
        var mockRepo = new Mock<IHistoricoAlteracaoRepository>();
        var mockMapper = new Mock<IMapper>();
        var mockFuncionarioRepository = new Mock<IFuncionarioRepository>();
        var mockCargoRepository = new Mock<ICargoRepository>();
        var mockFeriasRepository = new Mock<IFeriasRepository>();

        var model = new HistoricoAlteracaoModel { Id = 1, CampoAlterado = "Nome" };
        var readDTO = new HistoricoAlteracaoReadDTO { Id = 1, CampoAlterado = "Nome" };

        mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(model);
        mockMapper.Setup(m => m.Map<HistoricoAlteracaoReadDTO>(model)).Returns(readDTO);

        var service = GetService(mockRepo, mockFuncionarioRepository, mockCargoRepository, mockFeriasRepository, mockMapper);

        var result = await service.BuscarHistoricoPorId(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task DeletarHistorico_DeveRetornarTrue_QuandoSucesso()
    {
        var mockRepo = new Mock<IHistoricoAlteracaoRepository>();
        var mockMapper = new Mock<IMapper>();
        var mockFuncionarioRepository = new Mock<IFuncionarioRepository>();
        var mockCargoRepository = new Mock<ICargoRepository>();
        var mockFeriasRepository = new Mock<IFeriasRepository>();

        // Mock para existência do histórico
        mockRepo.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);
        mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        var service = GetService(mockRepo, mockFuncionarioRepository, mockCargoRepository, mockFeriasRepository, mockMapper);

        var result = await service.DeletarHistorico(1);

        Assert.True(result);
    }

    [Fact]
    public async Task ListarTodosHistoricos_DeveRetornarListaDeHistoricoReadDTO()
    {
        var mockRepo = new Mock<IHistoricoAlteracaoRepository>();
        var mockMapper = new Mock<IMapper>();
        var mockFuncionarioRepository = new Mock<IFuncionarioRepository>();
        var mockCargoRepository = new Mock<ICargoRepository>();
        var mockFeriasRepository = new Mock<IFeriasRepository>();

        var models = new List<HistoricoAlteracaoModel>
        {
            new HistoricoAlteracaoModel { DataAlteracao = DateTime.Now, ValorAntigo = "Gustavo", ValorNovo = "Vinícius", CampoAlterado = "Nome", IdFuncionario = 1, IdCargo = 1, IdFerias = 1 }
        };
        var dtos = new List<HistoricoAlteracaoReadDTO>
        {
            new HistoricoAlteracaoReadDTO { DataAlteracao = DateTime.Now, ValorAntigo = "Gustavo", ValorNovo = "Vinícius", CampoAlterado = "Nome", IdFuncionario = 1, IdCargo = 1, IdFerias = 1 }
        };

        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(models);
        mockMapper.Setup(m => m.Map<IEnumerable<HistoricoAlteracaoReadDTO>>(models)).Returns(dtos);

        var service = GetService(mockRepo, mockFuncionarioRepository, mockCargoRepository, mockFeriasRepository, mockMapper);

        var result = await service.ListarTodosHistoricos();

        Assert.NotNull(result);
        Assert.Empty(dtos);
    }
}