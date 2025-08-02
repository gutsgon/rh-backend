
namespace Rh_Backend.Models
{
    public class FuncionarioModel
    {
        public long Id { get; set; }
        public string Nome { get; set; } = "";
        public DateTime DataAdmissao { get; set; }
        public decimal Salario { get; set; }
        public bool Status { get; set; }
        public ICollection<FeriasModel> Ferias { get; set; } = new List<FeriasModel>();
        public ICollection<HistoricoAlteracaoModel> HistoricoAlteracao { get; set; } = new List<HistoricoAlteracaoModel>();
        public ICollection<ContratoModel> Contratos { get; set; } = new List<ContratoModel>();
    }
}