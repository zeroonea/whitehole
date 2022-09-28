namespace WhiteHole.DTO
{
    public class WhiteHoleObjectUpdateResponse
    {
        public string message { get; set;}
        public Dictionary<string, List<Dictionary<string, object>>> updated {  get; set; }
    }
}