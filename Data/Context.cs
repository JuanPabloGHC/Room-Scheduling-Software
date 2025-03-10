using Microsoft.EntityFrameworkCore;
using Room_Scheduling_Software.Data.Entities;

namespace Room_Scheduling_Software.Data
{
    public class Context : DbContext
    {
        #region < DB SETS DEFINITION >

        public DbSet<User> Users { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        #endregion

        #region < EVENT HANDLING >

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=<HOST>\\SQLEXPRESS;Database=GameRoomDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");
        }

        #endregion

    }

}