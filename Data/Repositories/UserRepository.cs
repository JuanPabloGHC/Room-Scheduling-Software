using Microsoft.EntityFrameworkCore;
using Room_Scheduling_Software.Data.Entities;
using Room_Scheduling_Software.Data.Interfaces;

namespace Room_Scheduling_Software.Data.Repositories
{
    public class UserRepository: IUserRepository
    {
        #region < DATA MEMBERS >

        private static UserRepository instance;

        #endregion

        #region < CONSTRUCTORS >

        private UserRepository() { }

        #endregion

        #region < PATTERN IMPLEMENTATIONS >

        #region < SINGLETON >

        public static UserRepository GetInstance()
        {
            if (instance == null)
                instance = new UserRepository();

            return instance;
        }

        #endregion

        #endregion

        #region < PUBLIC METHODS >

        public async Task<User?> GetEntity(string email)
        {
            using (var db = new Context())
            {
                return await db.Users
                    .Where(u => u.Email == email)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<User?> GetEntity(int userID)
        {
            using (var db = new Context())
            {
                return await db.Users
                    .Where(u => u.Id == userID)
                    .FirstOrDefaultAsync();
            }
        }

        public User Create(string name, string email, int number_visits)
        {
            using (var db = new Context())
            {
                User user = new User(name, email, number_visits);

                db.Add(user);

                db.SaveChanges();

                return user;
            }
        }

        public async Task UpdateVisits(int userID)
        {
            using (var db = new Context())
            {
                User? user = await this.GetEntity(userID);

                if (user == null)
                    return;

                user.Number_Visits += 1;

                db.SaveChanges();
            }
        }

        #endregion
    }
}