using Xunit;
using Moq;
using Rh_Backend.Services;
using Rh_Backend.Repository.Interfaces;
using Rh_Backend.DTO;
using AutoMapper;
using Rh_Backend.Models;
using Microsoft.Extensions.Logging;

public class CargoServiceTests
{
    private CargoService GetService(
        Mock<ICargoRepository> mockRepo,
        Mock<IContratoRepository> mockContratoRepo,
        Mock<IMapper> mockMapper,
        Mock<ILogger<CargoService>> mockLogger)
    {
        return new CargoService(
            mockRepo.Object,
            mockContratoRepo.Object,
            mockMapper.Object,
            mockLogger.Object
        );
    }

    [Fact]
    public async Task CriarCargo_DeveRetornarCargoReadDTO_QuandoSucesso()
    {
        var mockRepo = new Mock<ICargoRepository>();
        var mockContratoRepo = new Mock<IContratoRepository>();
        var mockMapper = new Mock<IMapper>();
        var mockLogger = new Mock<ILogger<CargoService>>();

        var cargoCreateDTO = new CargoCreateDTO { Nome = "Analista" };
        var cargoModel = new CargoModel { Id = 1, Nome = "Analista" };
        var cargoReadDTO = new CargoReadDTO { Id = 1, Nome = "Analista" };

        mockRepo.Setup(r => r.ExistsByNomeAsync("Analista")).ReturnsAsync(false);
        mockRepo.Setup(r => r.CreateAsync(cargoModel)).ReturnsAsync(cargoModel);
        mockMapper.Setup(m => m.Map<CargoModel>(cargoCreateDTO)).Returns(cargoModel);
        mockMapper.Setup(m => m.Map<CargoReadDTO>(cargoModel)).Returns(cargoReadDTO);

        var service = GetService(mockRepo, mockContratoRepo, mockMapper, mockLogger);

        var result = await service.CriarCargo(cargoCreateDTO);

        Assert.NotNull(result);
        Assert.Equal("Analista", result.Nome);
    }

    [Fact]
    public async Task BuscarCargoPorId_DeveRetornarCargoReadDTO_QuandoEncontrado()
    {
        var mockRepo = new Mock<ICargoRepository>();
        var mockContratoRepo = new Mock<IContratoRepository>();
        var mockMapper = new Mock<IMapper>();
        var mockLogger = new Mock<ILogger<CargoService>>();

        var cargoModel = new CargoModel { Id = 1, Nome = "Analista" };
        var cargoReadDTO = new CargoReadDTO { Id = 1, Nome = "Analista" };

        mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(cargoModel);
        mockMapper.Setup(m => m.Map<CargoReadDTO>(cargoModel)).Returns(cargoReadDTO);

        var service = GetService(mockRepo, mockContratoRepo, mockMapper, mockLogger);

        var result = await service.BuscarCargoPorId(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task BuscarCargoPorNome_DeveRetornarCargoReadDTO_QuandoEncontrado()
    {
        var mockRepo = new Mock<ICargoRepository>();
        var mockContratoRepo = new Mock<IContratoRepository>();
        var mockMapper = new Mock<IMapper>();
        var mockLogger = new Mock<ILogger<CargoService>>();

        var cargoModel = new CargoModel { Id = 1, Nome = "Analista" };
        var cargoReadDTO = new CargoReadDTO { Id = 1, Nome = "Analista" };

        mockRepo.Setup(r => r.GetByNomeAsync("Analista")).ReturnsAsync(cargoModel);
        mockMapper.Setup(m => m.Map<CargoReadDTO>(cargoModel)).Returns(cargoReadDTO);

        var service = GetService(mockRepo, mockContratoRepo, mockMapper, mockLogger);

        var result = await service.BuscarCargoPorNome("Analista");

        Assert.NotNull(result);
        Assert.Equal("Analista", result.Nome);
    }

    [Fact]
    public async Task AtualizarCargo_DeveRetornarCargoReadDTO_QuandoSucesso()
    {
        var mockRepo = new Mock<ICargoRepository>();
        var mockContratoRepo = new Mock<IContratoRepository>();
        var mockMapper = new Mock<IMapper>();
        var mockLogger = new Mock<ILogger<CargoService>>();

        var cargoUpdateDTO = new CargoUpdateDTO { Id = 1, Nome = "Analista Sênior" };
        var cargoModel = new CargoModel { Id = 1, Nome = "Analista Sênior" };
        var cargoReadDTO = new CargoReadDTO { Id = 1, Nome = "Analista Sênior" };

        mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(cargoModel);
        mockRepo.Setup(r => r.UpdateAsync(cargoModel)).ReturnsAsync(cargoModel);
        mockMapper.Setup(m => m.Map<CargoModel>(cargoUpdateDTO)).Returns(cargoModel);
        mockMapper.Setup(m => m.Map<CargoReadDTO>(cargoModel)).Returns(cargoReadDTO);

        var service = GetService(mockRepo, mockContratoRepo, mockMapper, mockLogger);

        var result = await service.AtualizarCargo(cargoUpdateDTO);

        Assert.NotNull(result);
        Assert.Equal("Analista Sênior", result.Nome);
    }

    [Fact]
    public async Task DeletarCargo_DeveRetornarTrue_QuandoSucesso()
    {
        var mockRepo = new Mock<ICargoRepository>();
        var mockContratoRepo = new Mock<IContratoRepository>();
        var mockMapper = new Mock<IMapper>();
        var mockLogger = new Mock<ILogger<CargoService>>();

        mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        var service = GetService(mockRepo, mockContratoRepo, mockMapper, mockLogger);

        var result = await service.DeletarCargo(1);

        Assert.True(result);
    }

    [Fact]
    public async Task ListarTodosCargos_DeveRetornarListaDeCargoReadDTO()
    {
        var mockRepo = new Mock<ICargoRepository>();
        var mockContratoRepo = new Mock<IContratoRepository>();
        var mockMapper = new Mock<IMapper>();
        var mockLogger = new Mock<ILogger<CargoService>>();

        var cargos = new List<CargoModel>
        {
            new CargoModel { Id = 1, Nome = "Analista" },
            new CargoModel { Id = 2, Nome = "Desenvolvedor" }
        };
        var cargosDTO = new List<CargoReadDTO>
        {
            new CargoReadDTO { Id = 1, Nome = "Analista" },
            new CargoReadDTO { Id = 2, Nome = "Desenvolvedor" }
        };

        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(cargos);
        mockMapper.Setup(m => m.Map<IEnumerable<CargoReadDTO>>(cargos)).Returns(cargosDTO);

        var service = GetService(mockRepo, mockContratoRepo, mockMapper, mockLogger);

        var result = await service.ListarTodosCargos();

        Assert.NotNull(result);
        Assert.Equal(2, ((List<CargoReadDTO>)result).Count);
    }
}