namespace Rh_Backend.DTO
{
    public class FuncionarioReadDTO
    {
        public long Id { get; set; }
        public string Nome { get; set; } = "";
        public DateTime DataAdmissao { get; set; }
        public decimal Salario { get; set; }
        public bool Status { get; set; }
    }
}