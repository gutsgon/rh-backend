namespace Rh_Backend.DTO
{
    public class FuncionarioComCargoDTO
    {
        public string Nome { get; set; } = "";
        public DateTime DataAdmissao { get; set; }
        public decimal Salario { get; set; }
        public bool Status { get; set; }
        public List<CargoCreateDTO> Cargos { get; set; } = new List<CargoCreateDTO>();
    }
}