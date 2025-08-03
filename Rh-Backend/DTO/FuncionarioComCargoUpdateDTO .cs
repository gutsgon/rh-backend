namespace Rh_Backend.DTO
{
    public class FuncionarioComCargoUpdateDTO
    {
        public long Id { get; set; }
        public string Nome { get; set; } = "";
        public DateTime DataAdmissao { get; set; }
        public decimal Salario { get; set; }
        public bool Status { get; set; }
        public CargoCreateDTO CargoAntigo { get; set; } = new CargoCreateDTO();
        public CargoCreateDTO CargoNovo { get; set; } = new CargoCreateDTO();
    }
}