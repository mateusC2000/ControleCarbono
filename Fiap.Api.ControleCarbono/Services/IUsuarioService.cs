using Fiap.Api.ControleCarbono.Models;

namespace Fiap.Api.ControleCarbono.Services
{
    public interface IUsuarioService
    {
        Task<Usuario?> Authenticate(string username, string password);
        Task<Usuario> Create(Usuario usuario, string password);
        Task<Usuario?> GetById(int id);
    }
}