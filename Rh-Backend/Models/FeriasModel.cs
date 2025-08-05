
namespace Rh_Backend.Models
{
    public class FeriasModel
    {
        public long Id { get; set; }
        public long IdFuncionario { get; set; }
        public FuncionarioModel Funcionario { get; set; }
        public ICollection<HistoricoAlteracaoModel> HistoricoAlteracao { get; set; } = new List<HistoricoAlteracaoModel>();
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string Status { get; set; } = "";
    }
}