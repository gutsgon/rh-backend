namespace Rh_Backend.DTO
{
    public class HistoricoAlteracaoReadDTO
    {
        public long Id { get; set; }
        public long IdFuncionario { get; set; }
        public long IdCargo { get; set; }
        public long IdFerias { get; set; }
        public DateTime DataAlteracao { get; set; }
        public string CampoAlterado { get; set; } = "";
        public string ValorAntigo { get; set; } = "";
        public string ValorNovo { get; set; } = "";
    }
}