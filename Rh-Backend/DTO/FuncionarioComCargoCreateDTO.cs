namespace Rh_Backend.DTO
{
    public class FuncionarioComCargoCreateDTO
    {
        public string Nome { get; set; } = "";
        public DateTime DataAdmissao { get; set; }
        public decimal Salario { get; set; }
        public bool Status { get; set; }
        public CargoCreateDTO Cargo { get; set; } = new CargoCreateDTO();
    }
}