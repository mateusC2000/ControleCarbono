using Fiap.Api.ControleCarbono.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fiap.Api.ControleCarbono.Services
{
    public interface IEmissaoCarbonoService
    {
        Task<List<EmissaoCarbono>> GetAllAsync();
        Task<List<EmissaoCarbono>> GetByEmpresaIdAsync(int empresaId);
        Task<EmissaoCarbono?> GetByIdAsync(int id);
        Task AddAsync(EmissaoCarbono emissao);
        Task<bool> UpdateAsync(int id, EmissaoCarbono emissao);
        Task<bool> RemoveAsync(int id);
    }
}