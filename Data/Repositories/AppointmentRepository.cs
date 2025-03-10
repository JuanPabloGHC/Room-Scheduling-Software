using Microsoft.EntityFrameworkCore;
using Room_Scheduling_Software.Data.Entities;
using Room_Scheduling_Software.Data.Interfaces;

namespace Room_Scheduling_Software.Data.Repositories
{
    public class AppointmentRepository: IAppointmentRepository
    {
        #region < DATA MEMBERS >

        private static AppointmentRepository instance;

        #endregion

        #region < CONSTRUCTORS >

        private AppointmentRepository() { }

        #endregion

        #region < PATTERN IMPLEMENTATIONS >

        #region < SINGLETON >

        public static AppointmentRepository GetInstance()
        {
            if (instance == null)
                instance = new AppointmentRepository();

            return instance;
        }

        #endregion

        #endregion

        #region < PUBLIC METHODS >

        public async Task<List<Appointment>> GetAll()
        {
            using (var db = new Context())
            {
                return await db.Appointments
                    .Include(a => a.User)
                    .Include(a => a.Room)
                    .ToListAsync();
            }
        }

        public async Task<List<Appointment?>> GetAllLast()
        {
            using (var db = new Context())
            {
                return await db.Appointments
                    .Include(a => a.User)
                    .Include(a => a.Room)
                    .GroupBy(a => a.Room)
                    .Select(g => g.OrderByDescending(a => a.Start_Hour).FirstOrDefault())
                    .ToListAsync();
            }
        }

        public async Task<Appointment?> GetEntity(int appointmentID)
        {
            using (var db = new Context())
            {
                return await db.Appointments
                    .Include(a => a.User)
                    .Include(a => a.Room)
                    .Where(a => a.Id == appointmentID)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<Appointment?> GetCurrent(int roomID)
        {
            using (var db = new Context())
            {
                return await db.Appointments
                    .Include(a => a.User)
                    .Include(a => a.Room)
                    .Where(a => a.RoomId == roomID && DateTime.Now < a.End_Hour)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<Appointment> Create(int roomID, int userID, DateTime? startHour, DateTime endHour, decimal price)
        {
            await UserRepository.GetInstance().UpdateVisits(userID);

            await RoomRepository.GetInstance().ModifyStatus(roomID, false);

            using (var db = new Context())
            {
                Appointment appointment = new Appointment(roomID, userID, endHour, price, startHour);

                db.Add(appointment);

                db.SaveChanges();

                return appointment;
            }
        }

        public async Task<Appointment?> Modify(int appointmentID, int userID, DateTime endHour, decimal price)
        {
            using (var db = new Context())
            {
                Appointment? appointment = await this.GetEntity(appointmentID);

                if (appointment == null)
                    return null;

                if (appointment.UserId != userID)
                    await UserRepository.GetInstance().UpdateVisits(userID);

                appointment.UserId = userID;
                appointment.End_Hour = endHour;
                appointment.Price = price;

                db.SaveChanges();

                return appointment;
            }
        }

        #endregion

    }
}