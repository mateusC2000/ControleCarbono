namespace Fiap.Api.ControleCarbono.ViewModel
{
    public class EmissaoCarbonoViewModel
    {
        public int Id { get; set; }
        public string Fonte { get; set; } = string.Empty;
        public decimal QuantidadeToneladas { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public int EmpresaId { get; set; }
    }
}
