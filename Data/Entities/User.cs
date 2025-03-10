using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Room_Scheduling_Software.Data.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public int Number_Visits { get; set; } = 0;

        public User() { }

        public User(string name, string email, int number_visits)
        {
            this.Name = name;
            this.Email = email;
            this.Number_Visits = number_visits;
        }

    }
}
