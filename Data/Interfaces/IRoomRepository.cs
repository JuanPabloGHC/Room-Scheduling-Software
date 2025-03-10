using Room_Scheduling_Software.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Room_Scheduling_Software.Data.Interfaces
{
    public interface IRoomRepository
    {
        public Task<List<Room>> GetAll();

        public Task<Room?> GetEntity(int roomID);

        public Room Create(int categoryID, string name, int capacity, decimal hourly_price, bool isFree);

        public Task<Room?> Modify(int roomID, int categoryID, string name, int capacity, decimal hourly_price);

        public Task ModifyStatus(int roomID, bool status);

        public Task Delete(int roomID);

    }
}