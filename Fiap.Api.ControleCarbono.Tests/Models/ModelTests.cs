using Xunit;
using Fiap.Api.ControleCarbono.Models;
using System;

namespace Fiap.Api.ControleCarbono.Tests.Models
{
    public class ModelTests
    {
        [Fact]
        public void EmissaoCarbono_Properties_CanBeSetAndGet()
        {
            var emissao = new EmissaoCarbono();
            var testDate = DateTime.Parse("2023-01-15");
            var empresa = new Empresa { Id = 101, Nome = "Empresa Teste" };

            emissao.Id = 1;
            emissao.Fonte = "Transporte";
            emissao.QuantidadeToneladas = 15.5m;
            emissao.Data = testDate;
            emissao.Descricao = "Viagens corporativas mensais";
            emissao.EmpresaId = 101;
            emissao.Empresa = empresa;

            Assert.Equal(1, emissao.Id);
            Assert.Equal("Transporte", emissao.Fonte);
            Assert.Equal(15.5m, emissao.QuantidadeToneladas);
            Assert.Equal(testDate, emissao.Data);
            Assert.Equal("Viagens corporativas mensais", emissao.Descricao);
            Assert.Equal(101, emissao.EmpresaId);
            Assert.Equal(empresa, emissao.Empresa);
        }

        [Fact]
        public void Empresa_Properties_CanBeSetAndGet()
        {
            var empresa = new Empresa();

            empresa.Id = 1;
            empresa.Nome = "Tech Solutions Ltda.";
            empresa.Cnpj = "12.345.678/0001-90";
            empresa.Username = "admin_empresa";
            empresa.Password = "SenhaSegura123";

            Assert.Equal(1, empresa.Id);
            Assert.Equal("Tech Solutions Ltda.", empresa.Nome);
            Assert.Equal("12.345.678/0001-90", empresa.Cnpj);
            Assert.Equal("admin_empresa", empresa.Username);
            Assert.Equal("SenhaSegura123", empresa.Password);
        }

        [Fact]
        public void Usuario_Properties_CanBeSetAndGet()
        {
            var usuario = new Usuario();

            usuario.Id = 1;
            usuario.Nome = "João Silva";
            usuario.Email = "joao.silva@example.com";
            usuario.Username = "jsilva";
            usuario.PasswordHash = "hashedpasswordxyz";
            usuario.Role = "Admin";

            Assert.Equal(1, usuario.Id);
            Assert.Equal("João Silva", usuario.Nome);
            Assert.Equal("joao.silva@example.com", usuario.Email);
            Assert.Equal("jsilva", usuario.Username);
            Assert.Equal("hashedpasswordxyz", usuario.PasswordHash);
            Assert.Equal("Admin", usuario.Role);
        }

        [Fact]
        public void Usuario_DefaultRole_ShouldBeUser()
        {
            var usuario = new Usuario();

            Assert.Equal("User", usuario.Role);
        }
    }
}
