using Xunit;
using Moq;
using AutoMapper;
using Fiap.Api.ControleCarbono.Controllers;
using Fiap.Api.ControleCarbono.Services;
using Fiap.Api.ControleCarbono.Models;
using Fiap.Api.ControleCarbono.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EmissaoCarbonoControllerTests
{
    private readonly Mock<IEmissaoCarbonoService> _serviceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly EmissaoCarbonoController _controller;

    public EmissaoCarbonoControllerTests()
    {
        _serviceMock = new Mock<IEmissaoCarbonoService>();
        _mapperMock = new Mock<IMapper>();
        _controller = new EmissaoCarbonoController(_serviceMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnOkWithEmissoes()
    {
        var emissoes = new List<EmissaoCarbono> { new EmissaoCarbono { Id = 1 } };
        var viewModels = new List<EmissaoCarbonoViewModel> { new EmissaoCarbonoViewModel { Id = 1 } };
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(emissoes);
        _mapperMock.Setup(m => m.Map<IEnumerable<EmissaoCarbonoViewModel>>(emissoes)).Returns(viewModels);

        var result = await _controller.Get();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<EmissaoCarbonoViewModel>>(okResult.Value);
        Assert.Single(returnValue);
    }

    [Fact]
    public async Task GetById_ShouldReturnOkWithEmissao_WhenFound()
    {
        var emissao = new EmissaoCarbono { Id = 1 };
        var viewModel = new EmissaoCarbonoViewModel { Id = 1 };
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(emissao);
        _mapperMock.Setup(m => m.Map<EmissaoCarbonoViewModel>(emissao)).Returns(viewModel);

        var result = await _controller.Get(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<EmissaoCarbonoViewModel>(okResult.Value);
        Assert.Equal(1, returnValue.Id);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenNotFound()
    {
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((EmissaoCarbono)null);

        var result = await _controller.Get(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetByEmpresa_ShouldReturnOkWithEmissoes()
    {
        var emissoes = new List<EmissaoCarbono> { new EmissaoCarbono { Id = 1, EmpresaId = 5 } };
        var viewModels = new List<EmissaoCarbonoViewModel> { new EmissaoCarbonoViewModel { Id = 1, EmpresaId = 5 } };
        _serviceMock.Setup(s => s.GetByEmpresaIdAsync(5)).ReturnsAsync(emissoes);
        _mapperMock.Setup(m => m.Map<IEnumerable<EmissaoCarbonoViewModel>>(emissoes)).Returns(viewModels);

        var result = await _controller.GetByEmpresa(5);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<EmissaoCarbonoViewModel>>(okResult.Value);
        Assert.Single(returnValue);
    }

    [Fact]
    public async Task Post_ShouldReturnCreatedAtAction()
    {
        var viewModel = new EmissaoCarbonoViewModel { Fonte = "Teste" };
        var emissao = new EmissaoCarbono { Fonte = "Teste" };
        _mapperMock.Setup(m => m.Map<EmissaoCarbono>(viewModel)).Returns(emissao);
        _mapperMock.Setup(m => m.Map<EmissaoCarbonoViewModel>(emissao)).Returns(viewModel);
        _serviceMock.Setup(s => s.AddAsync(emissao)).Returns(Task.CompletedTask).Callback<EmissaoCarbono>(e => e.Id = 1);

        var result = await _controller.Post(viewModel);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(_controller.Get), createdAtActionResult.ActionName);
        Assert.Equal(1, createdAtActionResult.RouteValues["id"]);
    }

    [Fact]
    public async Task Put_ShouldReturnNoContent_WhenSuccess()
    {
        var viewModel = new EmissaoCarbonoViewModel { Id = 1, Fonte = "Update" };
        var emissao = new EmissaoCarbono { Id = 1, Fonte = "Update" };
        _mapperMock.Setup(m => m.Map<EmissaoCarbono>(viewModel)).Returns(emissao);
        _serviceMock.Setup(s => s.UpdateAsync(1, It.IsAny<EmissaoCarbono>())).ReturnsAsync(true);

        var result = await _controller.Put(1, viewModel);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Put_ShouldReturnNotFound_WhenFail()
    {
        var viewModel = new EmissaoCarbonoViewModel { Id = 999 };
        _mapperMock.Setup(m => m.Map<EmissaoCarbono>(viewModel)).Returns(new EmissaoCarbono());
        _serviceMock.Setup(s => s.UpdateAsync(999, It.IsAny<EmissaoCarbono>())).ReturnsAsync(false);

        var result = await _controller.Put(999, viewModel);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenSuccess()
    {
        _serviceMock.Setup(s => s.RemoveAsync(1)).ReturnsAsync(true);

        var result = await _controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenFail()
    {
        _serviceMock.Setup(s => s.RemoveAsync(999)).ReturnsAsync(false);

        var result = await _controller.Delete(999);

        Assert.IsType<NotFoundResult>(result);
    }
}
