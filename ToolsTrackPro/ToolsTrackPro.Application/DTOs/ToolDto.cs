namespace ToolsTrackPro.Application.DTOs
{
    public class ToolDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public byte StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
