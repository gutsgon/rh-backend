namespace Rh_Backend.Models
{
    public class HistoricoAlteracaoModel
    {
        public long Id { get; set; }
        public long IdFuncionario { get; set; }
        public long IdCargo { get; set; }
        public long IdFerias { get; set; }
        public FuncionarioModel Funcionario { get; set; } 
        public CargoModel Cargo { get; set; } 
        public FeriasModel Ferias { get; set; }
        public DateTime DataAlteracao { get; set; }
        public string CampoAlterado { get; set; } = "";
        public string ValorAntigo { get; set; } = "";
        public string ValorNovo { get; set; } = "";
    }
}