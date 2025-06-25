using Fiap.Api.ControleCarbono.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiap.Api.ControleCarbono.Services
{
    public class EmissaoCarbonoService : IEmissaoCarbonoService
    {
        private static readonly List<EmissaoCarbono> _emissoes = new();
        private static int _nextId = 1;

        public Task<List<EmissaoCarbono>> GetAllAsync()
        {
            return Task.FromResult(_emissoes.ToList());
        }

        public Task<List<EmissaoCarbono>> GetByEmpresaIdAsync(int empresaId)
        {
            var emissoes = _emissoes.Where(e => e.EmpresaId == empresaId).ToList();
            return Task.FromResult(emissoes);
        }

        public Task<EmissaoCarbono?> GetByIdAsync(int id)
        {
            var emissao = _emissoes.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(emissao);
        }

        public Task AddAsync(EmissaoCarbono emissao)
        {
            emissao.Id = _nextId++;
            _emissoes.Add(emissao);
            return Task.CompletedTask;
        }

        public Task<bool> UpdateAsync(int id, EmissaoCarbono emissao)
        {
            var index = _emissoes.FindIndex(e => e.Id == id);
            if (index == -1)
                return Task.FromResult(false);

            emissao.Id = id;
            _emissoes[index] = emissao;
            return Task.FromResult(true);
        }

        public Task<bool> RemoveAsync(int id)
        {
            var emissao = _emissoes.FirstOrDefault(e => e.Id == id);
            if (emissao == null)
                return Task.FromResult(false);

            _emissoes.Remove(emissao);
            return Task.FromResult(true);
        }
    }
}