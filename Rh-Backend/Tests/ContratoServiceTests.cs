using Xunit;
using Moq;
using Rh_Backend.Services;
using Rh_Backend.Repository.Interfaces;
using Rh_Backend.DTO;
using AutoMapper;
using Rh_Backend.Models;

public class ContratoServiceTests
{
    private ContratoService GetService(
        Mock<IContratoRepository> mockRepo,
        Mock<IFuncionarioRepository> mockFuncionarioRepository,
        Mock<ICargoRepository> mockCargoRepository,      
        Mock<IMapper> mockMapper)
    {
        return new ContratoService(
            mockRepo.Object,
            mockFuncionarioRepository.Object,
            mockCargoRepository.Object,
            mockMapper.Object
        );
    }

    [Fact]
    public async Task CriarContrato_DeveRetornarContratoDTO_QuandoSucesso()
    {
        var mockRepo = new Mock<IContratoRepository>();
        var mockMapper = new Mock<IMapper>();
        var mockFuncionarioRepository = new Mock<IFuncionarioRepository>();
        var mockCargoRepository = new Mock<ICargoRepository>();

        var createDTO = new ContratoDTO { IdFuncionario = 1, IdCargo = 1 };
        var model = new ContratoModel { IdFuncionario = 1, IdCargo = 1 };
        var readDTO = new ContratoDTO { IdFuncionario = 1, IdCargo = 1 };

        // ...dentro do método de teste...
        mockFuncionarioRepository.Setup(r => r.ExistsAsync(It.IsAny<long>())).ReturnsAsync(true);
        mockCargoRepository.Setup(r => r.ExistsAsync(It.IsAny<long>())).ReturnsAsync(true);
        mockRepo.Setup(r => r.ExistsAsync(It.IsAny<long>(), It.IsAny<long>())).ReturnsAsync(true);

        mockRepo.Setup(r => r.CreateAsync(It.IsAny<ContratoModel>())).ReturnsAsync(model);
        mockMapper.Setup(m => m.Map<ContratoModel>(createDTO)).Returns(model);
        mockMapper.Setup(m => m.Map<ContratoDTO>(model)).Returns(readDTO);

        var service = GetService(mockRepo, mockFuncionarioRepository, mockCargoRepository, mockMapper);

        var result = await service.CriarContrato(createDTO);

        Assert.NotNull(result);
        Assert.Equal(1, result.IdFuncionario);
        Assert.Equal(1, result.IdCargo);
    }

    [Fact]
    public async Task BuscarContratoPorId_DeveRetornarContratoDTO_QuandoEncontrado()
    {
        var mockRepo = new Mock<IContratoRepository>();
        var mockMapper = new Mock<IMapper>();
        var mockFuncionarioRepository = new Mock<IFuncionarioRepository>();
        var mockCargoRepository = new Mock<ICargoRepository>();

        var model = new ContratoModel { IdFuncionario = 1, IdCargo = 1 };
        var readDTO = new ContratoDTO { IdFuncionario = 1, IdCargo = 1 };

        mockRepo.Setup(r => r.GetByIdAsync(1, 1)).ReturnsAsync(model);
        mockMapper.Setup(m => m.Map<ContratoDTO>(model)).Returns(readDTO);

        var service = GetService(mockRepo, mockFuncionarioRepository, mockCargoRepository, mockMapper);

        var result = await service.BuscarContratoPorId(1, 1);

        Assert.NotNull(result);
        Assert.Equal(1, result.IdFuncionario);
        Assert.Equal(1, result.IdCargo);
    }

    [Fact]
    public async Task AtualizarContrato_DeveRetornarContratoDTO_QuandoSucesso()
    {
        var mockRepo = new Mock<IContratoRepository>();
        var mockMapper = new Mock<IMapper>();
        var mockFuncionarioRepository = new Mock<IFuncionarioRepository>();
        var mockCargoRepository = new Mock<ICargoRepository>();

        var updateDTO = new ContratoDTO { IdFuncionario = 1, IdCargo = 2 };
        var model = new ContratoModel { IdFuncionario = 1, IdCargo = 2 };
        var readDTO = new ContratoDTO { IdFuncionario = 1, IdCargo = 2 };

        mockRepo.Setup(r => r.GetByIdAsync(1, 2)).ReturnsAsync(model);
        mockRepo.Setup(r => r.UpdateAsync(updateDTO.IdFuncionario, updateDTO.IdCargo, 3)).ReturnsAsync(model);
        mockMapper.Setup(m => m.Map<ContratoModel>(updateDTO)).Returns(model);
        mockMapper.Setup(m => m.Map<ContratoDTO>(model)).Returns(readDTO);

        var service = GetService(mockRepo, mockFuncionarioRepository, mockCargoRepository, mockMapper);

        var result = await service.AtualizarContrato(1, 1, 2);

        Assert.NotNull(result);
        Assert.Equal(2, result.IdCargo);
    }

    [Fact]
    public async Task DeletarContrato_DeveRetornarTrue_QuandoSucesso()
    {
        var mockRepo = new Mock<IContratoRepository>();
        var mockMapper = new Mock<IMapper>();
        var mockFuncionarioRepository = new Mock<IFuncionarioRepository>();
        var mockCargoRepository = new Mock<ICargoRepository>();

        mockRepo.Setup(r => r.DeleteAsync(1, 2)).ReturnsAsync(true);

        var service = GetService(mockRepo, mockFuncionarioRepository, mockCargoRepository, mockMapper);

        var result = await service.DeletarContrato(1, 2);

        Assert.True(result);
    }

    [Fact]
    public async Task ListarTodosContratos_DeveRetornarListaDeContratoDTO()
    {
        var mockRepo = new Mock<IContratoRepository>();
        var mockMapper = new Mock<IMapper>();
        var mockFuncionarioRepository = new Mock<IFuncionarioRepository>();
        var mockCargoRepository = new Mock<ICargoRepository>();

        var models = new List<ContratoModel>
        {
            new ContratoModel { IdFuncionario = 1, IdCargo = 1 },
            new ContratoModel { IdFuncionario = 2, IdCargo = 2 }
        };
        var dtos = new List<ContratoDTO>
        {
            new ContratoDTO { IdFuncionario = 1, IdCargo = 1 },
            new ContratoDTO { IdFuncionario = 2, IdCargo = 2 }
        };

        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(models);
        mockMapper.Setup(m => m.Map<IEnumerable<ContratoDTO>>(models)).Returns(dtos);

        var service = GetService(mockRepo, mockFuncionarioRepository, mockCargoRepository, mockMapper);

        var result = await service.ListarTodosContratos();

        Assert.NotNull(result);
        Assert.Equal(2, ((List<ContratoDTO>)result).Count);
    }
}