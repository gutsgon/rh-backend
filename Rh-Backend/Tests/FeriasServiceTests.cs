using Xunit;
using Moq;
using Rh_Backend.Services;
using Rh_Backend.Repository.Interfaces;
using Rh_Backend.DTO;
using AutoMapper;
using Rh_Backend.Models;

public class FeriasServiceTests
{
    private FeriasService GetService(
        Mock<IFeriasRepository> mockRepo,
        Mock<IFuncionarioRepository> mockFuncionarioRepository,
        Mock<IMapper> mockMapper)
    {
        return new FeriasService(
            mockRepo.Object,
            mockMapper.Object,
            mockFuncionarioRepository.Object
        );
    }

    [Fact]
    public async Task CriarFerias_DeveRetornarFeriasReadDTO_QuandoSucesso()
    {
        var mockRepo = new Mock<IFeriasRepository>();
        var mockMapper = new Mock<IMapper>();
        var mockFuncionarioRepository = new Mock<IFuncionarioRepository>();

        var createDTO = new FeriasCreateDTO {DataInicio = new DateTime().AddDays(-1), DataFim = new DateTime().AddDays(3), IdFuncionario = 1};
        var model = new FeriasModel { Id = 1, Status = "Pendente" };
        var readDTO = new FeriasReadDTO { Id = 1, Status = "Pendente" };

        mockFuncionarioRepository.Setup(r => r.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(new FuncionarioModel { Id = 1, Nome = "Teste" });
        mockRepo.Setup(r => r.ExistsAsync(It.IsAny<long>())).ReturnsAsync(true);

        mockRepo.Setup(r => r.CreateAsync(It.IsAny<FeriasModel>())).ReturnsAsync(model);
        mockMapper.Setup(m => m.Map<FeriasModel>(createDTO)).Returns(model);
        mockMapper.Setup(m => m.Map<FeriasReadDTO>(model)).Returns(readDTO);

        var service = GetService(mockRepo, mockFuncionarioRepository, mockMapper);

        var result = await service.CriarFerias(createDTO);

        Assert.NotNull(result);
        Assert.Equal("Pendente", result.Status);
    }

    [Fact]
    public async Task BuscarFeriasPorId_DeveRetornarFeriasReadDTO_QuandoEncontrado()
    {
        var mockRepo = new Mock<IFeriasRepository>();
        var mockMapper = new Mock<IMapper>();
         var mockFuncionarioRepository = new Mock<IFuncionarioRepository>();

        var model = new FeriasModel { Id = 1, Status = "Pendente" };
        var readDTO = new FeriasReadDTO { Id = 1, Status = "Pendente" };

        mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(model);
        mockMapper.Setup(m => m.Map<FeriasReadDTO>(model)).Returns(readDTO);

        var service = GetService(mockRepo, mockFuncionarioRepository, mockMapper);

        var result = await service.BuscarFeriasPorId(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task AtualizarFerias_DeveRetornarFeriasReadDTO_QuandoSucesso()
    {
        var mockRepo = new Mock<IFeriasRepository>();
        var mockMapper = new Mock<IMapper>();
         var mockFuncionarioRepository = new Mock<IFuncionarioRepository>();

        var updateDTO = new FeriasUpdateDTO {Id = 1, DataFim = new DateTime().AddDays(3) };
        var model = new FeriasModel { Id = 1, Status = "Concluídas" };
        var readDTO = new FeriasReadDTO { Id = 1, Status = "Concluídas" };

        mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(model);
        mockRepo.Setup(r => r.UpdateAsync(It.IsAny<FeriasModel>())).ReturnsAsync(model);
        mockMapper.Setup(m => m.Map<FeriasModel>(updateDTO)).Returns(model);
        mockMapper.Setup(m => m.Map<FeriasReadDTO>(model)).Returns(readDTO);

        var service = GetService(mockRepo, mockFuncionarioRepository, mockMapper);

        var result = await service.AtualizarFerias(updateDTO);

        Assert.NotNull(result);
        Assert.Equal("Concluídas", result.Status);
    }

    [Fact]
    public async Task DeletarFerias_DeveRetornarTrue_QuandoSucesso()
    {
        var mockRepo = new Mock<IFeriasRepository>();
        var mockMapper = new Mock<IMapper>();
        var mockFuncionarioRepository = new Mock<IFuncionarioRepository>();

        mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        var service = GetService(mockRepo, mockFuncionarioRepository, mockMapper);

        var result = await service.DeletarFerias(1);

        Assert.True(result);
    }

    [Fact]
    public async Task ListarTodasFerias_DeveRetornarListaDeFeriasReadDTO()
    {
        var mockRepo = new Mock<IFeriasRepository>();
        var mockMapper = new Mock<IMapper>();
         var mockFuncionarioRepository = new Mock<IFuncionarioRepository>();

        var models = new List<FeriasModel>
        {
            new FeriasModel { Id = 1, Status = "Pendente" },
            new FeriasModel { Id = 2, Status = "Concluídas" }
        };
        var dtos = new List<FeriasReadDTO>
        {
            new FeriasReadDTO { Id = 1, Status = "Pendente" },
            new FeriasReadDTO { Id = 2, Status = "Concluídas" }
        };

        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(models);
        mockMapper.Setup(m => m.Map<IEnumerable<FeriasReadDTO>>(models)).Returns(dtos);

        var service = GetService(mockRepo, mockFuncionarioRepository, mockMapper);

        var result = await service.ListarTodasFerias();

        Assert.NotNull(result);
        Assert.Equal(2, ((List<FeriasReadDTO>)result).Count);
    }
}