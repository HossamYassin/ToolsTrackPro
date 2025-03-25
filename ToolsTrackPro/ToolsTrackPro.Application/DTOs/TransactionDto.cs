namespace ToolsTrackPro.Application.DTOs
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ToolId { get; set; }
        public required DateTime BorrowDate { get; set; }
        public required DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public byte StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
