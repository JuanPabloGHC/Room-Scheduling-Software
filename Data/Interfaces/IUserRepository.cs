using Microsoft.EntityFrameworkCore;
using Room_Scheduling_Software.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Room_Scheduling_Software.Data.Interfaces
{
    public interface IUserRepository
    {
        public Task<User?> GetEntity(string email);

        public Task<User?> GetEntity(int userID);

        public User Create(string name, string email, int number_visits);

        public Task UpdateVisits(int userID);

    }
}