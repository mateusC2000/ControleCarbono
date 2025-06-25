using System.Security.Cryptography;
using System.Text;
using Fiap.Api.ControleCarbono.Models;

namespace Fiap.Api.ControleCarbono.Services
{
    public class UsuarioService : IUsuarioService
    {
        private static List<Usuario> _usuarios = new();
        
        public Task<Usuario?> Authenticate(string username, string password)
        {
            var usuario = _usuarios.SingleOrDefault(u => u.Username == username);
            
            if (usuario == null || !VerifyPasswordHash(password, usuario.PasswordHash))
                return Task.FromResult<Usuario?>(null);

            return Task.FromResult<Usuario?>(usuario);
        }

        public Task<Usuario> Create(Usuario usuario, string password)
        {
            if (_usuarios.Any(u => u.Username == usuario.Username))
                throw new Exception("Username já existe");

            usuario.PasswordHash = CreatePasswordHash(password);
            usuario.Id = _usuarios.Count + 1;
            
            _usuarios.Add(usuario);
            return Task.FromResult(usuario);
        }

        public Task<Usuario?> GetById(int id)
        {
            return Task.FromResult(_usuarios.FirstOrDefault(u => u.Id == id));
        }

        private static string CreatePasswordHash(string password)
        {
            using var hmac = new HMACSHA512();
            var passwordSalt = hmac.Key;
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            
            // Combine salt and hash for storage
            return Convert.ToBase64String(passwordSalt) + "|" + Convert.ToBase64String(passwordHash);
        }

        private static bool VerifyPasswordHash(string password, string storedHash)
        {
            var parts = storedHash.Split('|');
            if (parts.Length != 2) return false;
            
            var passwordSalt = Convert.FromBase64String(parts[0]);
            var passwordHash = Convert.FromBase64String(parts[1]);
            
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}