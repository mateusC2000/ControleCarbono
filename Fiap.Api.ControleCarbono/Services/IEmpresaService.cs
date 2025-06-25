using Fiap.Api.ControleCarbono.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fiap.Api.ControleCarbono.Services
{
    public interface IEmpresaService
    {
        Task<List<Empresa>> GetAllAsync();
        Task<Empresa?> GetByIdAsync(int id);
        Task<Empresa?> GetByCnpjAsync(string cnpj);
        Task AddAsync(Empresa empresa);
        Task<bool> UpdateAsync(int id, Empresa empresa);
        Task<bool> RemoveAsync(int id);
    }
}
