using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Fiap.Api.ControleCarbono.Services;
using Fiap.Api.ControleCarbono.Models;
using Fiap.Api.ControleCarbono.ViewModel;
using Fiap.Api.ControleCarbono.Controllers;
using System.Threading.Tasks;
using System.Reflection;

namespace Fiap.Api.ControleCarbono.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly Mock<IUsuarioService> _mockUsuarioService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _mockUsuarioService = new Mock<IUsuarioService>();
            _controller = new AuthController(_mockAuthService.Object, _mockUsuarioService.Object);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkWithToken()
        {
            var loginViewModel = new LoginViewModel { Username = "testuser", Password = "password123" };
            var usuarioRetorno = new Usuario 
            { 
                Id = 1, 
                Username = "testuser", 
                Nome = "Test User", 
                Email = "test@example.com", 
                Role = "Admin" 
            };
            var tokenGerado = "sample.jwt.token";

            _mockUsuarioService.Setup(s => s.Authenticate(loginViewModel.Username, loginViewModel.Password))
                               .ReturnsAsync(usuarioRetorno);

            _mockAuthService.Setup(s => s.GenerateToken(usuarioRetorno))
                            .Returns(tokenGerado);

            var result = await _controller.Login(loginViewModel);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);

            var returnedObject = okResult.Value;
            Assert.NotNull(returnedObject);

            var idProperty = returnedObject.GetType().GetProperty("Id");
            Assert.NotNull(idProperty);
            Assert.Equal(usuarioRetorno.Id, (int)idProperty.GetValue(returnedObject));

            var usernameProperty = returnedObject.GetType().GetProperty("Username");
            Assert.NotNull(usernameProperty);
            Assert.Equal(usuarioRetorno.Username, (string)usernameProperty.GetValue(returnedObject));

            var nomeProperty = returnedObject.GetType().GetProperty("Nome");
            Assert.NotNull(nomeProperty);
            Assert.Equal(usuarioRetorno.Nome, (string)nomeProperty.GetValue(returnedObject));

            var emailProperty = returnedObject.GetType().GetProperty("Email");
            Assert.NotNull(emailProperty);
            Assert.Equal(usuarioRetorno.Email, (string)emailProperty.GetValue(returnedObject));

            var roleProperty = returnedObject.GetType().GetProperty("Role");
            Assert.NotNull(roleProperty);
            Assert.Equal(usuarioRetorno.Role, (string)roleProperty.GetValue(returnedObject));

            var tokenProperty = returnedObject.GetType().GetProperty("Token");
            Assert.NotNull(tokenProperty);
            Assert.Equal(tokenGerado, (string)tokenProperty.GetValue(returnedObject));

            _mockUsuarioService.Verify(s => s.Authenticate(loginViewModel.Username, loginViewModel.Password), Times.Once);
            _mockAuthService.Verify(s => s.GenerateToken(usuarioRetorno), Times.Once);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            var loginViewModel = new LoginViewModel { Username = "wronguser", Password = "wrongpassword" };

            _mockUsuarioService.Setup(s => s.Authenticate(loginViewModel.Username, loginViewModel.Password))
                               .ReturnsAsync((Usuario)null); 

            var result = await _controller.Login(loginViewModel);

            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.NotNull(unauthorizedResult.Value);

            var errorMessageObject = unauthorizedResult.Value;
            Assert.NotNull(errorMessageObject);

            var messageProperty = errorMessageObject.GetType().GetProperty("message");
            Assert.NotNull(messageProperty);
            Assert.Equal("Usuário ou senha incorretos", (string)messageProperty.GetValue(errorMessageObject));

            _mockUsuarioService.Verify(s => s.Authenticate(loginViewModel.Username, loginViewModel.Password), Times.Once);
            _mockAuthService.Verify(s => s.GenerateToken(It.IsAny<Usuario>()), Times.Never);
        }
    }
}
