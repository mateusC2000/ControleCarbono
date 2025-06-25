using System;
using Xunit;
using Fiap.Api.ControleCarbono.ViewModel;

namespace Fiap.Tests.ViewModels
{
    public class ViewModelTests
    {
        [Fact]
        public void EmissaoCarbonoViewModel_Should_Set_Properties_Correctly()
        {
            var emissao = new EmissaoCarbonoViewModel
            {
                Id = 1,
                Fonte = "Indústria",
                QuantidadeToneladas = 12.5m,
                Data = new DateTime(2024, 1, 1),
                Descricao = "Emissão anual",
                EmpresaId = 100
            };

            Assert.Equal(1, emissao.Id);
            Assert.Equal("Indústria", emissao.Fonte);
            Assert.Equal(12.5m, emissao.QuantidadeToneladas);
            Assert.Equal(new DateTime(2024, 1, 1), emissao.Data);
            Assert.Equal("Emissão anual", emissao.Descricao);
            Assert.Equal(100, emissao.EmpresaId);
        }

        [Fact]
        public void EmpresaViewModel_Should_Set_Properties_Correctly()
        {
            var empresa = new EmpresaViewModel
            {
                Id = 10,
                Nome = "FiapTech",
                Cnpj = "12.345.678/0001-90",
                Username = "fiapuser",
                Password = "123456"
            };

            Assert.Equal(10, empresa.Id);
            Assert.Equal("FiapTech", empresa.Nome);
            Assert.Equal("12.345.678/0001-90", empresa.Cnpj);
            Assert.Equal("fiapuser", empresa.Username);
            Assert.Equal("123456", empresa.Password);
        }

        [Fact]
        public void LoginViewModel_Should_Set_Properties_Correctly()
        {
            var login = new LoginViewModel
            {
                Username = "admin",
                Password = "senha123"
            };

            Assert.Equal("admin", login.Username);
            Assert.Equal("senha123", login.Password);
        }

        [Fact]
        public void UsuarioViewModel_Should_Set_Properties_Correctly()
        {
            var usuario = new UsuarioViewModel
            {
                Nome = "João Silva",
                Email = "joao@email.com",
                Username = "joaosilva",
                Password = "senhaSegura"
            };

            Assert.Equal("João Silva", usuario.Nome);
            Assert.Equal("joao@email.com", usuario.Email);
            Assert.Equal("joaosilva", usuario.Username);
            Assert.Equal("senhaSegura", usuario.Password);
        }
    }
}
