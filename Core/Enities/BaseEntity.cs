using System.ComponentModel.DataAnnotations;

namespace Core.Enities
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
