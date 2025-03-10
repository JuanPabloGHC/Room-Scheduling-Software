using Room_Scheduling_Software.Data.Entities;

namespace Room_Scheduling_Software.Data.Interfaces
{
    public interface IUserRepository
    {
        #region < PUBLIC METHODS >

        public Task<User?> GetEntity(string email);

        public Task<User?> GetEntity(int userID);

        public User Create(string name, string email, int number_visits);

        public Task UpdateVisits(int userID);

        #endregion

    }
}