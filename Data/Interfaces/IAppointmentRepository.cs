using Room_Scheduling_Software.Data.Entities;

namespace Room_Scheduling_Software.Data.Interfaces
{
    public interface IAppointmentRepository
    {
        #region < PUBLIC METHODS >

        public Task<List<Appointment>> GetAll();

        public Task<List<Appointment?>> GetAllLast();

        public Task<Appointment?> GetCurrent(int roomID);

        public Task<Appointment> Create(int roomID, int userID, DateTime? startHour, DateTime endHour, decimal price);

        public Task<Appointment?> Modify(int appointmentID, int userID, DateTime endHour, decimal price);

        #endregion

    }
}