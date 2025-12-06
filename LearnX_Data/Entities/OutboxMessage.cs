using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_Data.Entities
{
    public class OutboxMessage
    {
        [Key] public Guid Id { get; set; } = Guid.NewGuid();
        [Required, MaxLength(128)] public string EventType { get; set; } = default!;
        [Required] public string Payload { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PublishedAt { get; set; }
        public int PublishAttempts { get; set; }
        public string? Error { get; set; }
    }
}