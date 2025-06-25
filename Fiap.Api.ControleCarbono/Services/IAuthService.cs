using Fiap.Api.ControleCarbono.Models;

namespace Fiap.Api.ControleCarbono.Services
{
    public interface IAuthService
    {
        string GenerateToken(Usuario usuario);
    }
}
