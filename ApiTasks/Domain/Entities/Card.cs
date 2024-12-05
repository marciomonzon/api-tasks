using Domain.Enums;

namespace Domain.Entities
{
    public class Card
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public StatusCardEnum Status { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? DeadLine { get; set; }
        public ListCard? List { get; set; }
    }
}
