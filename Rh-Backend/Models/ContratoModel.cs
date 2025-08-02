
namespace Rh_Backend.Models
{
    public class ContratoModel
    {
        public long IdFuncionario { get; set; }
        public long IdCargo { get; set; }

        public FuncionarioModel Funcionario { get; set; } = new FuncionarioModel();
        public CargoModel Cargo { get; set; } = new CargoModel();
    }
}