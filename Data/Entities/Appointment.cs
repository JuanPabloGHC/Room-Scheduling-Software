using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Room_Scheduling_Software.Data.Entities
{
    [Table("Appointments")]
    public class Appointment
    {
        #region < PROPERTIES >

        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }

        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public Room? Room { get; set; }

        public DateTime? Start_Hour { get; set; }

        public DateTime? End_Hour { get; set; }

        public decimal Price { get; set; }

        #endregion

        #region < CONSTRUCTORS >

        public Appointment() { }

        public Appointment(int roomID, int userID, DateTime endHour, decimal price, DateTime? startHour = null)
        {
            this.UserId = userID;
            this.RoomId = roomID;
            this.Start_Hour = startHour != null ? startHour : DateTime.Now;
            this.End_Hour = endHour;
            this.Price = price;
        }

        #endregion

    }
}