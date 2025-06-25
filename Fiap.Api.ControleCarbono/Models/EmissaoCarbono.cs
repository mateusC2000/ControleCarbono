namespace Fiap.Api.ControleCarbono.Models
{
    public class EmissaoCarbono
    {
        public int Id { get; set; }
        public string Fonte { get; set; } = string.Empty;
        public decimal QuantidadeToneladas { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public int EmpresaId { get; set; }
        public Empresa? Empresa { get; set; }
    }
}