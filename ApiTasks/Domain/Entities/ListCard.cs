namespace Domain.Entities
{
    public class ListCard
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public StatusItemEnum Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Workspace? Workspace { get; set; }
        public ICollection<Card>? Cards { get; set; }
    }
}
