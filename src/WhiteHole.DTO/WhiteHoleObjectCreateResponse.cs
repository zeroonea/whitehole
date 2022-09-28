namespace WhiteHole.DTO
{
    public class WhiteHoleObjectCreateResponse
    {
        public string message { get; set;}
        public Dictionary<string, List<Dictionary<string, object>>> created {  get; set; }
    }
}