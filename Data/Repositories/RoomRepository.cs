using Microsoft.EntityFrameworkCore;
using Room_Scheduling_Software.Data.Entities;
using Room_Scheduling_Software.Data.Interfaces;

namespace Room_Scheduling_Software.Data.Repositories
{
    public class RoomRepository: IRoomRepository
    {
        #region < DATA MEMBERS >

        private static RoomRepository instance;

        #endregion

        #region < CONSTRUCTORS >

        private RoomRepository() { }

        #endregion

        #region < PATTERN IMPLEMENTATIONS >

        #region < SINGLETON >

        public static RoomRepository GetInstance()
        {
            if (instance == null)
                instance = new RoomRepository();

            return instance;
        }

        #endregion

        #endregion

        #region < PUBLIC METHODS >

        public async Task<List<Room>> GetAll()
        {
            using (var db = new Context())
            {
                return await db.Rooms
                    .Include(r => r.Category)
                    .ToListAsync();
            }
        }

        public async Task<Room?> GetEntity(int roomID)
        {
            using (var db = new Context())
            {
                return await db.Rooms
                    .Where(r => r.Id == roomID)
                    .Include(r => r.Category)
                    .FirstOrDefaultAsync();
            }
        }

        public Room Create(int categoryID, string name, int capacity, decimal hourly_price, bool isFree)
        {
            using (var db = new Context())
            {
                Room room = new Room(categoryID, name, capacity, hourly_price, isFree);

                db.Add(room);

                db.SaveChanges();

                return room;
            }
        }

        public async Task<Room?> Modify(int roomID, int categoryID, string name, int capacity, decimal hourly_price)
        {
            using (var db = new Context())
            {
                Room? room = await this.GetEntity(roomID);

                if (room == null)
                    return null;

                room.Name = name;
                room.Capacity = capacity;
                room.Hourly_Price = hourly_price;

                room.CategoryId = categoryID;

                db.SaveChanges();

                return room;
            }
        }

        public async Task ModifyStatus(int roomID, bool status)
        {
            Room? room = await this.GetEntity(roomID);

            if (room == null)
                return;

            using (var db = new Context())
            {
                room.IsFree = status;

                await db.SaveChangesAsync();
            }
            
        }

        public async Task Delete(int roomID)
        {
            Room? room = await this.GetEntity(roomID);

            if (room == null)
                return;

            using (var db = new Context())
            {
                db.Remove(roomID);

                await db.SaveChangesAsync();
            }
        }

        #endregion

    }
}