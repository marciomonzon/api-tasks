using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Workspace
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(45, MinimumLength = 2)]
        public string? Title { get; set; }
        public User? User { get; set; }
        public ICollection<ListCard>? ListCards { get; set; }
        public StatusItemEnum Status { get; set; } = StatusItemEnum.Active;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
