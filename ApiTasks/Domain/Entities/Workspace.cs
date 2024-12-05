using Domain.Enums;

namespace Domain.Entities
{
    public class Workspace
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public User? User { get; set; }
        public ICollection<ListCard>? ListCards { get; set; }
        public StatusItemEnum Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
