using Xunit;
using Moq;
using Rh_Backend.Services;
using Rh_Backend.Repository.Interfaces;
using Rh_Backend.DTO;
using AutoMapper;
using Rh_Backend.Models;
using Microsoft.Extensions.Logging;
using Rh_Backend.Data;
using Rh_Backend.Repository;

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
    public async Task CriarCargo_DeveRetornarCargoCriado()
    {
        var context = TestHelper.GetInMemoryDbContext();

        var cargoRepository = new CargoRepository(context);
        var contratoRepository = new ContratoRepository(context);

        var mockMapper = new Mock<IMapper>();
        var mockLogger = new Mock<ILogger<CargoService>>();

        var cargoDto = new CargoCreateDTO { Nome = "Desenvolvedor" };

        var cargoEntity = new CargoModel { Id = 1, Nome = "Desenvolvedor" };
        var cargoReadDto = new CargoReadDTO { Id = 1, Nome = "Desenvolvedor" };

        mockMapper.Setup(m => m.Map<CargoModel>(cargoDto)).Returns(cargoEntity);
        mockMapper.Setup(m => m.Map<CargoReadDTO>(cargoEntity)).Returns(cargoReadDto);

        var service = new CargoService(
            cargoRepository,
            contratoRepository,
            mockMapper.Object,
            mockLogger.Object
        );

        var result = await service.CriarCargo(cargoDto);

        Assert.NotNull(result);
        Assert.Equal("Desenvolvedor", result.Nome);
    }
        [Fact]
        public async Task BuscarCargoPorId_DeveRetornarCargoReadDTO_QuandoEncontrado()
        {
            // Arrange
            var context = TestHelper.GetInMemoryDbContext();

            // Adiciona cargo no contexto in-memory
            context.Add(new CargoModel { Id = 1, Nome = "Analista" });
            await context.SaveChangesAsync();

            var cargoRepository = new CargoRepository(context);
            var contratoRepository = new ContratoRepository(context);

            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<CargoService>>();

            var cargoModel = new CargoModel { Id = 1, Nome = "Analista" };
            var cargoReadDTO = new CargoReadDTO { Id = 1, Nome = "Analista" };

            mockMapper.Setup(m => m.Map<CargoReadDTO>(It.Is<CargoModel>(c => c.Id == 1))).Returns(cargoReadDTO);

            var service = new CargoService(cargoRepository, contratoRepository, mockMapper.Object, mockLogger.Object);

            // Act
            var result = await service.BuscarCargoPorId(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Analista", result.Nome);
        }

        [Fact]
        public async Task BuscarCargoPorNome_DeveRetornarCargoReadDTO_QuandoEncontrado()
        {
            // Arrange
            var context = TestHelper.GetInMemoryDbContext();

            context.Add(new CargoModel { Id = 1, Nome = "Analista" });
            await context.SaveChangesAsync();

            var cargoRepository = new CargoRepository(context);
            var contratoRepository = new ContratoRepository(context);

            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<CargoService>>();

            var cargoModel = new CargoModel { Id = 1, Nome = "Analista" };
            var cargoReadDTO = new CargoReadDTO { Id = 1, Nome = "Analista" };

            mockMapper.Setup(m => m.Map<CargoReadDTO>(It.Is<CargoModel>(c => c.Nome == "Analista"))).Returns(cargoReadDTO);

            var service = new CargoService(cargoRepository, contratoRepository, mockMapper.Object, mockLogger.Object);

            // Act
            var result = await service.BuscarCargoPorNome("Analista");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Analista", result.Nome);
        }

        [Fact]
        public async Task AtualizarCargo_DeveRetornarCargoReadDTO_QuandoSucesso()
        {
            // Arrange
            var context = TestHelper.GetInMemoryDbContext();

            // Cargo inicial
            var cargoOriginal = new CargoModel { Id = 1, Nome = "Analista" };
            context.Add(cargoOriginal);
            await context.SaveChangesAsync();

            var cargoRepository = new CargoRepository(context);
            var contratoRepository = new ContratoRepository(context);

            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<CargoService>>();

            var cargoUpdateDTO = new CargoUpdateDTO { Id = 1, Nome = "Analista Sênior" };
            var cargoAtualizado = new CargoModel { Id = 1, Nome = "Analista Sênior" };
            var cargoReadDTO = new CargoReadDTO { Id = 1, Nome = "Analista Sênior" };

            // Setup mapper para converter UpdateDTO -> Model
            mockMapper.Setup(m => m.Map<CargoModel>(cargoUpdateDTO)).Returns(cargoAtualizado);
            // Setup mapper para converter Model -> ReadDTO
            mockMapper.Setup(m => m.Map<CargoReadDTO>(cargoAtualizado)).Returns(cargoReadDTO);

            var service = new CargoService(cargoRepository, contratoRepository, mockMapper.Object, mockLogger.Object);

            // Act
            var result = await service.AtualizarCargo(cargoUpdateDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Analista Sênior", result.Nome);
        }

        [Fact]
        public async Task DeletarCargo_DeveRetornarTrue_QuandoSucesso()
        {
            // Arrange
            var context = TestHelper.GetInMemoryDbContext();

            context.Add(new CargoModel { Id = 1, Nome = "Analista" });
            await context.SaveChangesAsync();

            var cargoRepository = new CargoRepository(context);
            var contratoRepository = new ContratoRepository(context);

            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<CargoService>>();

            var service = new CargoService(cargoRepository, contratoRepository, mockMapper.Object, mockLogger.Object);

            // Act
            var result = await service.DeletarCargo(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ListarTodosCargos_DeveRetornarListaDeCargoReadDTO()
        {
            // Arrange
            var context = TestHelper.GetInMemoryDbContext();

            var cargos = new List<CargoModel>
        {
            new CargoModel { Id = 1, Nome = "Analista" },
            new CargoModel { Id = 2, Nome = "Desenvolvedor" }
        };

            context.AddRange(cargos);
            await context.SaveChangesAsync();

            var cargoRepository = new CargoRepository(context);
            var contratoRepository = new ContratoRepository(context);

            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<CargoService>>();

            var cargosDTO = new List<CargoReadDTO>
        {
            new CargoReadDTO { Id = 1, Nome = "Analista" },
            new CargoReadDTO { Id = 2, Nome = "Desenvolvedor" }
        };

            mockMapper.Setup(m => m.Map<IEnumerable<CargoReadDTO>>(It.IsAny<IEnumerable<CargoModel>>())).Returns(cargosDTO);

            var service = new CargoService(cargoRepository, contratoRepository, mockMapper.Object, mockLogger.Object);

            // Act
            var result = await service.ListarTodosCargos();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
}
