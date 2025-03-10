using Room_Scheduling_Software.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Room_Scheduling_Software.Data.Interfaces
{
    public interface IAppointmentRepository
    {
        public Task<List<Appointment>> GetAll();

        public Task<List<Appointment?>> GetAllLast();

        public Task<Appointment?> GetCurrent(int roomID);

        public Task<Appointment> Create(int roomID, int userID, DateTime? startHour, DateTime endHour, decimal price);

        public Task<Appointment?> Modify(int appointmentID, int userID, DateTime endHour, decimal price);

    }
}