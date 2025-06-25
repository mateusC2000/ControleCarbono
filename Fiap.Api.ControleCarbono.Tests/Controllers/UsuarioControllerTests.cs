using Xunit;
using Moq;
using Fiap.Api.ControleCarbono.Controllers;
using Fiap.Api.ControleCarbono.Services;
using Fiap.Api.ControleCarbono.ViewModel;
using Fiap.Api.ControleCarbono.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Text.Json;

namespace Fiap.Api.ControleCarbono.Tests
{
    public class RegisterSuccessResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }

    public class ErrorResponse
    {
        public string Message { get; set; }
    }

    public class UsuarioControllerTests
    {
        private readonly Mock<IUsuarioService> _mockUsuarioService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UsuarioController _controller;

        public UsuarioControllerTests()
        {
            _mockUsuarioService = new Mock<IUsuarioService>();
            _mockMapper = new Mock<IMapper>();
            _controller = new UsuarioController(_mockUsuarioService.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Register_ShouldReturnOk_WhenRegistrationIsSuccessful()
        {
            var viewModel = new UsuarioViewModel { Username = "newuser", Password = "password123" };
            var user = new Usuario { Id = 1, Username = "newuser" };

            _mockMapper.Setup(m => m.Map<Usuario>(It.IsAny<UsuarioViewModel>())).Returns(user);
            _mockUsuarioService.Setup(s => s.Create(It.IsAny<Usuario>(), It.IsAny<string>())).ReturnsAsync(user);

            var result = await _controller.Register(viewModel);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var json = JsonSerializer.Serialize(okResult.Value);
            var returnValue = JsonSerializer.Deserialize<RegisterSuccessResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(returnValue);
            Assert.Equal(user.Id, returnValue.Id);
            Assert.Equal(user.Username, returnValue.Username);
        }

        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenRegistrationFails()
        {
            var viewModel = new UsuarioViewModel { Username = "existinguser", Password = "password123" };
            var user = new Usuario { Username = "existinguser" };

            _mockMapper.Setup(m => m.Map<Usuario>(It.IsAny<UsuarioViewModel>())).Returns(user);
            _mockUsuarioService.Setup(s => s.Create(It.IsAny<Usuario>(), It.IsAny<string>())).ThrowsAsync(new Exception("Username já existe"));

            var result = await _controller.Register(viewModel);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var json = JsonSerializer.Serialize(badRequestResult.Value);
            var errorValue = JsonSerializer.Deserialize<ErrorResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(errorValue);
            Assert.Contains("Username já existe", errorValue.Message);
        }

        [Fact]
        public void GetAll_ShouldReturnOk()
        {
            var result = _controller.GetAll();

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenUserExists()
        {
            var userId = 1;
            var user = new Usuario { Id = userId, Username = "testuser" };

            _mockUsuarioService.Setup(s => s.GetById(userId)).ReturnsAsync(user);

            var result = await _controller.GetById(userId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<Usuario>(okResult.Value);
            Assert.Equal(userId, returnedUser.Id);
            Assert.Equal(user.Username, returnedUser.Username);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            var userId = 999;

            _mockUsuarioService.Setup(s => s.GetById(userId)).ReturnsAsync((Usuario)null);

            var result = await _controller.GetById(userId);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
