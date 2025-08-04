

namespace Rh_Backend.Models
{
    public class CargoModel
    {
        public long Id { get; set; }
        public string Nome { get; set; } = "";
        public ICollection<ContratoModel> Contratos { get; set; } = new List<ContratoModel>();
        public ICollection<HistoricoAlteracaoModel> HistoricoAlteracao { get; set; } = new List<HistoricoAlteracaoModel>();
    }
}