using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
