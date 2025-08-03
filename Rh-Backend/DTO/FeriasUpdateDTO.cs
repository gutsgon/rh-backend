namespace Rh_Backend.DTO
{
    public class FeriasUpdateDTO
    {
        public long Id { get; set; }
        public long IdFuncionario { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string Status { get; set; } = "";
    }
}