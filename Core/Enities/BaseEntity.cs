using System.ComponentModel.DataAnnotations;

namespace Core.Enities
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        protected virtual void ValidateString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(nameof(value), "Value can't be null");
        }
    }
}
