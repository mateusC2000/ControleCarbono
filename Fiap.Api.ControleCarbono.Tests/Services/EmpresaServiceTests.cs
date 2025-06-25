using Xunit;
using Fiap.Api.ControleCarbono.Models;
using Fiap.Api.ControleCarbono.Services;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

public class EmpresaServiceTests
{
    private readonly EmpresaService _service;

    public EmpresaServiceTests()
    {
        var empresasField = typeof(EmpresaService).GetField("_empresas", BindingFlags.NonPublic | BindingFlags.Static);
        empresasField?.SetValue(null, new List<Empresa>());
        
        var nextIdField = typeof(EmpresaService).GetField("_nextId", BindingFlags.NonPublic | BindingFlags.Static);
        nextIdField?.SetValue(null, 1);
        
        _service = new EmpresaService();
    }

    [Fact]
    public async Task AddAsync_ShouldAddEmpresaAndAssignId()
    {
        var empresa = new Empresa { Nome = "FIAP", Cnpj = "12.345.678/0001-99" };

        await _service.AddAsync(empresa);
        var result = await _service.GetByIdAsync(empresa.Id);

        Assert.NotNull(result);
        Assert.Equal(1, empresa.Id);
        Assert.Equal("FIAP", result.Nome);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEmpresas()
    {
        await _service.AddAsync(new Empresa { Nome = "Empresa A" });
        await _service.AddAsync(new Empresa { Nome = "Empresa B" });

        var result = await _service.GetAllAsync();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectEmpresa_WhenFound()
    {
        var empresa = new Empresa { Nome = "Empresa Teste" };
        await _service.AddAsync(empresa);

        var result = await _service.GetByIdAsync(empresa.Id);

        Assert.NotNull(result);
        Assert.Equal(empresa.Id, result.Id);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        var result = await _service.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByCnpjAsync_ShouldReturnCorrectEmpresa_WhenFound()
    {
        string cnpj = "98.765.432/0001-11";
        await _service.AddAsync(new Empresa { Nome = "Empresa CNPJ", Cnpj = cnpj });

        var result = await _service.GetByCnpjAsync(cnpj);

        Assert.NotNull(result);
        Assert.Equal(cnpj, result.Cnpj);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEmpresa_WhenFound()
    {
        var empresa = new Empresa { Nome = "Nome Original" };
        await _service.AddAsync(empresa);
        var updatedEmpresa = new Empresa { Nome = "Nome Atualizado", Cnpj = "11.111.111/0001-11" };

        var success = await _service.UpdateAsync(empresa.Id, updatedEmpresa);
        var result = await _service.GetByIdAsync(empresa.Id);

        Assert.True(success);
        Assert.NotNull(result);
        Assert.Equal("Nome Atualizado", result.Nome);
        Assert.Equal(empresa.Id, result.Id);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenNotFound()
    {
        var updatedEmpresa = new Empresa { Nome = "Inexistente" };

        var success = await _service.UpdateAsync(999, updatedEmpresa);
        
        Assert.False(success);
    }

    [Fact]
    public async Task RemoveAsync_ShouldRemoveEmpresa_WhenFound()
    {
        var empresa = new Empresa { Nome = "Empresa a ser removida" };
        await _service.AddAsync(empresa);

        var success = await _service.RemoveAsync(empresa.Id);
        var result = await _service.GetByIdAsync(empresa.Id);
        var allEmpresas = await _service.GetAllAsync();

        Assert.True(success);
        Assert.Null(result);
        Assert.Empty(allEmpresas);
    }
    
    [Fact]
    public async Task RemoveAsync_ShouldReturnFalse_WhenNotFound()
    {
        var success = await _service.RemoveAsync(999);
        
        Assert.False(success);
    }
}
