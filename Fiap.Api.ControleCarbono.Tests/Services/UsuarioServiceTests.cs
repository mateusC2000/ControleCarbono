using Xunit;
using Moq;
using Fiap.Api.ControleCarbono.Services;
using Fiap.Api.ControleCarbono.Models;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Fiap.Api.ControleCarbono.Tests
{
    public class UsuarioServiceTests
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioServiceTests()
        {
            _usuarioService = new UsuarioService();

            var usuariosField = typeof(UsuarioService).GetField("_usuarios", BindingFlags.Static | BindingFlags.NonPublic);
            if (usuariosField != null)
            {
                usuariosField.SetValue(null, new List<Usuario>());
            }
        }

        [Fact]
        public async Task Create_ShouldCreateNewUser_WhenUsernameDoesNotExist()
        {
            var user = new Usuario { Username = "testuser" };
            var password = "password123";

            var createdUser = await _usuarioService.Create(user, password);

            Assert.NotNull(createdUser);
            Assert.True(createdUser.Id > 0);
            Assert.Equal("testuser", createdUser.Username);
            Assert.NotNull(createdUser.PasswordHash);

            var retrievedUser = await _usuarioService.GetById(createdUser.Id);
            Assert.NotNull(retrievedUser);
            Assert.Equal(createdUser.Id, retrievedUser.Id);
        }

        [Fact]
        public async Task Create_ShouldThrowException_WhenUsernameAlreadyExists()
        {
            var user1 = new Usuario { Username = "existinguser" };
            var password = "password123";
            await _usuarioService.Create(user1, password);

            var user2 = new Usuario { Username = "existinguser" };

            var exception = await Assert.ThrowsAsync<Exception>(() => _usuarioService.Create(user2, password));
            Assert.Equal("Username já existe", exception.Message);
        }

        [Fact]
        public async Task Authenticate_ShouldReturnUser_WhenCredentialsAreValid()
        {
            var username = "authuser";
            var password = "authpassword";
            var user = new Usuario { Username = username };
            await _usuarioService.Create(user, password);

            var authenticatedUser = await _usuarioService.Authenticate(username, password);

            Assert.NotNull(authenticatedUser);
            Assert.Equal(username, authenticatedUser.Username);
        }

        [Fact]
        public async Task Authenticate_ShouldReturnNull_WhenUsernameIsInvalid()
        {
            var username = "nonexistent";
            var password = "anypassword";

            var authenticatedUser = await _usuarioService.Authenticate(username, password);

            Assert.Null(authenticatedUser);
        }

        [Fact]
        public async Task Authenticate_ShouldReturnNull_WhenPasswordIsInvalid()
        {
            var username = "authuser2";
            var password = "correctpassword";
            var wrongPassword = "wrongpassword";
            var user = new Usuario { Username = username };
            await _usuarioService.Create(user, password);

            var authenticatedUser = await _usuarioService.Authenticate(username, wrongPassword);

            Assert.Null(authenticatedUser);
        }

        [Fact]
        public async Task GetById_ShouldReturnUser_WhenUserExists()
        {
            var user = new Usuario { Username = "getbyiduser" };
            var password = "password123";
            var createdUser = await _usuarioService.Create(user, password);

            var retrievedUser = await _usuarioService.GetById(createdUser.Id);

            Assert.NotNull(retrievedUser);
            Assert.Equal(createdUser.Id, retrievedUser.Id);
            Assert.Equal(createdUser.Username, retrievedUser.Username);
        }

        [Fact]
        public async Task GetById_ShouldReturnNull_WhenUserDoesNotExist()
        {
            var nonExistentId = 999;

            var retrievedUser = await _usuarioService.GetById(nonExistentId);

            Assert.Null(retrievedUser);
        }
    }
}
