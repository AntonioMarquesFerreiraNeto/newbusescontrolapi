using BusesControl.Entities.Models.v1;
using BusesControl.Persistence.Repositories.Interfaces.v1;
using Moq;

namespace BusesControl.UnitTests.Services.v1;

public class ColorServiceTest
{
    private readonly Mock<IColorRepository> _colorRepositoryMock;
    private readonly List<ColorModel> _colors;

    public ColorServiceTest()
    {
        _colorRepositoryMock = new Mock<IColorRepository>();

        _colors = new List<ColorModel>
        {
            new() { Id = Guid.Parse("b7bcfda2-032d-4c35-8703-49f1d42544cd"), Active = true, Color = "Vermelho" },
            new() { Id = Guid.Parse("71d38f52-aa81-4d9a-9169-b8753adf651c"), Active = true, Color = "Azul" },
            new() { Id = Guid.Parse("5c2583d4-3379-40b6-a510-b04b05db361a"), Active = true, Color = "Verde" },
            new() { Id = Guid.Parse("1678ac20-3444-488c-bbd3-8f4ee17f2ddd"), Active = true, Color = "Verde" }
        };
    }

    [Theory(DisplayName = "GetById_DeveRetornarCor_QuandoIdExiste")]
    [InlineData("b7bcfda2-032d-4c35-8703-49f1d42544cd")]
    [InlineData("1678ac20-3444-488c-bbd3-8f4ee17f2ddd")]
    [InlineData("5c2583d4-3379-40b6-a510-b04b05db361a")]
    [InlineData("71d38f52-aa81-4d9a-9169-b8753adf651c")]
    public async Task GetById(Guid id)
    {
        _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(_colors.SingleOrDefault(x => x.Id == id));
        var record = await _colorRepositoryMock.Object.GetByIdAsync(id);

        Assert.NotNull(record);
    }

    [Fact(DisplayName = "FindBySearch_DeveRetornarListaDeCores_QuandoBuscaValida")]
    public async Task FindBySearch()
    {
        _colorRepositoryMock.Setup(repo => repo.FindBySearchAsync(0, 10, null)).ReturnsAsync(_colors);
        var records = await _colorRepositoryMock.Object.FindBySearchAsync(0, 10, null);

        Assert.NotNull(records);
    }

    [Fact(DisplayName = "Create_DeveCriarNovaCor_QuandoDadosValidos")]
    public async Task Create()
    {
        var record = new ColorModel
        {
            Color = "Roxo"
        };

        _colorRepositoryMock.Setup(repo => repo.AddAsync(record));
        await _colorRepositoryMock.Object.AddAsync(record);
        _colorRepositoryMock.Verify(repo => repo.AddAsync(record), Times.Once);
    }

    [Theory(DisplayName = "Update_DeveAtualizarCor_QuandoDadosValidos")]
    [InlineData("b7bcfda2-032d-4c35-8703-49f1d42544cd")]
    [InlineData("1678ac20-3444-488c-bbd3-8f4ee17f2ddd")]
    [InlineData("5c2583d4-3379-40b6-a510-b04b05db361a")]
    [InlineData("71d38f52-aa81-4d9a-9169-b8753adf651c")]
    public async Task Update(Guid id)
    {
        var recordMock = _colors.SingleOrDefault(x => x.Id == id);
        _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(recordMock);

        var record = await _colorRepositoryMock.Object.GetByIdAsync(id);

        Assert.NotNull(record);

        _colorRepositoryMock.Setup(repo => repo.Update(record)).Verifiable();
        _colorRepositoryMock.Object.Update(record);
        _colorRepositoryMock.Verify(repo => repo.Update(record), Times.Once);
    }

    [Theory(DisplayName = "ToggleActive_DeveAlterarAtivacao_QuandoEstadoValido")]
    [InlineData("b7bcfda2-032d-4c35-8703-49f1d42544cd")]
    [InlineData("1678ac20-3444-488c-bbd3-8f4ee17f2ddd")]
    [InlineData("5c2583d4-3379-40b6-a510-b04b05db361a")]
    [InlineData("71d38f52-aa81-4d9a-9169-b8753adf651c")]
    public async Task ToggleActive(Guid id)
    {
        var recordMock = _colors.SingleOrDefault(x => x.Id == id);
        _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(recordMock);

        var record = await _colorRepositoryMock.Object.GetByIdAsync(id);

        Assert.NotNull(record);

        record.Active = !record.Active;

        _colorRepositoryMock.Setup(repo => repo.Update(record)).Verifiable();
        _colorRepositoryMock.Object.Update(record);

        _colorRepositoryMock.Verify(repo => repo.Update(record), Times.Once);
    }

    [Theory(DisplayName = "Delete_DeveRemoverCor_QuandoIdValido")]
    [InlineData("b7bcfda2-032d-4c35-8703-49f1d42544cd")]
    [InlineData("1678ac20-3444-488c-bbd3-8f4ee17f2ddd")]
    [InlineData("5c2583d4-3379-40b6-a510-b04b05db361a")]
    [InlineData("71d38f52-aa81-4d9a-9169-b8753adf651c")]
    public async Task Delete(Guid id)
    {
        var recordMock = _colors.SingleOrDefault(x => x.Id == id);
        _colorRepositoryMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(recordMock);

        var record = await _colorRepositoryMock.Object.GetByIdAsync(id);
        Assert.NotNull(record);

        _colorRepositoryMock.Setup(repo => repo.Remove(record)).Verifiable();
        _colorRepositoryMock.Object.Remove(record);

        _colorRepositoryMock.Verify(repo => repo.Remove(record), Times.Once);
    }
}
