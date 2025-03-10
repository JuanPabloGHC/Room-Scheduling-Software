using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Room_Scheduling_Software.Data.Entities
{
    [Table("Categories")]
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        public byte[]? Photo { get; set; }

        public ICollection<Room>? Rooms { get; set; }

        public Category() { }

        public Category(string name, byte[] photo)
        {
            this.Name = name;
            this.Photo = photo;
        }
    }
}
