using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using Fiap.Api.ControleCarbono.Services;
using Fiap.Api.ControleCarbono.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;

namespace Fiap.Api.ControleCarbono.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("ThisIsAVerySecureSecretKeyForTestingPurposes12345");
            _authService = new AuthService(_mockConfiguration.Object);
        }

        [Fact]
        public void GenerateToken_ValidUser_ReturnsNonEmptyToken()
        {
            var usuario = new Usuario
            {
                Id = 1,
                Username = "testuser",
                Nome = "Test User",
                Email = "test@example.com",
                Role = "User"
            };

            var token = _authService.GenerateToken(usuario);

            Assert.False(string.IsNullOrEmpty(token));

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            Assert.NotNull(jwtToken);
            Assert.Contains(jwtToken.Claims, c => c.Type == "unique_name" && c.Value == usuario.Username);
            Assert.Contains(jwtToken.Claims, c => c.Type == "role" && c.Value == usuario.Role);

            _mockConfiguration.Verify(c => c["Jwt:Key"], Times.Once);
        }
    }
}
