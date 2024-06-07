using Microsoft.EntityFrameworkCore;
using Room_Scheduling_Software.Data.Entities;
using System.Diagnostics;

namespace Room_Scheduling_Software.Data;

public class Context : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Room> Rooms { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Appointment> Appointments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=<HOST>;Database=GameRoomDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");
    }
}
