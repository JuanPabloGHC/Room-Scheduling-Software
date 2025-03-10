using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Room_Scheduling_Software.Data.Entities
{
    [Table("Users")]
    public class User
    {
        #region < PROPERTIES >

        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public int Number_Visits { get; set; } = 0;

        #endregion

        #region < CONSTRUCTORS >
        public User() { }

        public User(string name, string email, int number_visits)
        {
            this.Name = name;
            this.Email = email;
            this.Number_Visits = number_visits;
        }

        #endregion

    }
}