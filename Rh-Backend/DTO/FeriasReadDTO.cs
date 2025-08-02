namespace Rh_Backend.DTO
{
    public class FeriasReadDTO
    {
        public long Id { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string Status { get; set; } = "";
    }
}