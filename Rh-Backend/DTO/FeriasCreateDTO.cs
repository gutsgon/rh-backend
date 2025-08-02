namespace Rh_Backend.DTO
{
    public class FeriasCreateDTO
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string Status { get; set; } = "";
    }
}