using Fiap.Api.ControleCarbono.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiap.Api.ControleCarbono.Services
{
    public class EmpresaService : IEmpresaService
    {
        private static List<Empresa> _empresas = new();
        private static int _nextId = 1;

        public Task<List<Empresa>> GetAllAsync()
        {
            return Task.FromResult(_empresas.ToList());
        }

        public Task<Empresa?> GetByIdAsync(int id)
        {
            var empresa = _empresas.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(empresa);
        }

        public Task<Empresa?> GetByCnpjAsync(string cnpj)
        {
            var empresa = _empresas.FirstOrDefault(e => e.Cnpj == cnpj);
            return Task.FromResult(empresa);
        }

        public Task AddAsync(Empresa empresa)
        {
            empresa.Id = _nextId++;
            _empresas.Add(empresa);
            return Task.CompletedTask;
        }

        public Task<bool> UpdateAsync(int id, Empresa empresa)
        {
            var index = _empresas.FindIndex(e => e.Id == id);
            if (index == -1)
                return Task.FromResult(false);

            empresa.Id = id;
            _empresas[index] = empresa;
            return Task.FromResult(true);
        }

        public Task<bool> RemoveAsync(int id)
        {
            var empresa = _empresas.FirstOrDefault(e => e.Id == id);
            if (empresa == null)
                return Task.FromResult(false);

            _empresas.Remove(empresa);
            return Task.FromResult(true);
        }
    }
}
