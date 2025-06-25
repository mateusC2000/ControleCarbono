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

public class EmpresaControllerTests
{
    private readonly Mock<IEmpresaService> _serviceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly EmpresaController _controller;

    public EmpresaControllerTests()
    {
        _serviceMock = new Mock<IEmpresaService>();
        _mapperMock = new Mock<IMapper>();
        _controller = new EmpresaController(_serviceMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnOkWithEmpresas()
    {
        var empresas = new List<Empresa> { new Empresa { Id = 1, Nome = "Empresa A" } };
        var viewModels = new List<EmpresaViewModel> { new EmpresaViewModel { Id = 1, Nome = "Empresa A" } };
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(empresas);
        _mapperMock.Setup(m => m.Map<IEnumerable<EmpresaViewModel>>(empresas)).Returns(viewModels);

        var result = await _controller.Get();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<EmpresaViewModel>>(okResult.Value);
        Assert.Single(returnValue);
    }

    [Fact]
    public async Task GetById_ShouldReturnOkWithEmpresa_WhenFound()
    {
        var empresa = new Empresa { Id = 1, Nome = "Empresa B" };
        var viewModel = new EmpresaViewModel { Id = 1, Nome = "Empresa B" };
        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(empresa);
        _mapperMock.Setup(m => m.Map<EmpresaViewModel>(empresa)).Returns(viewModel);

        var result = await _controller.Get(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<EmpresaViewModel>(okResult.Value);
        Assert.Equal(1, returnValue.Id);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenNotFound()
    {
        _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Empresa)null);

        var result = await _controller.Get(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Post_ShouldReturnCreatedAtAction()
    {
        var viewModel = new EmpresaViewModel { Nome = "Nova Empresa" };
        var empresa = new Empresa { Nome = "Nova Empresa" };
        _mapperMock.Setup(m => m.Map<Empresa>(viewModel)).Returns(empresa);
        _mapperMock.Setup(m => m.Map<EmpresaViewModel>(empresa)).Returns(viewModel);
        _serviceMock.Setup(s => s.AddAsync(empresa)).Returns(Task.CompletedTask).Callback<Empresa>(e => e.Id = 10);

        var result = await _controller.Post(viewModel);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(_controller.Get), createdAtActionResult.ActionName);
        Assert.Equal(10, createdAtActionResult.RouteValues["id"]);
        Assert.Equal(viewModel, createdAtActionResult.Value);
    }

    [Fact]
    public async Task Put_ShouldReturnNoContent_WhenSuccess()
    {
        var viewModel = new EmpresaViewModel { Id = 1, Nome = "Nome Atualizado" };
        var empresa = new Empresa();
        _mapperMock.Setup(m => m.Map<Empresa>(viewModel)).Returns(empresa);
        _serviceMock.Setup(s => s.UpdateAsync(1, It.IsAny<Empresa>())).ReturnsAsync(true);

        var result = await _controller.Put(1, viewModel);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Put_ShouldReturnNotFound_WhenFail()
    {
        var viewModel = new EmpresaViewModel { Id = 999 };
        _mapperMock.Setup(m => m.Map<Empresa>(viewModel)).Returns(new Empresa());
        _serviceMock.Setup(s => s.UpdateAsync(999, It.IsAny<Empresa>())).ReturnsAsync(false);

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
