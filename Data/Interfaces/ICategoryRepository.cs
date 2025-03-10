using Room_Scheduling_Software.Data.Entities;

namespace Room_Scheduling_Software.Data.Interfaces
{
    public interface ICategoryRepository
    {
        #region < PUBLIC METHODS >

        public Task<List<Category>> GetAll();

        public Task<Category?> GetEntity(int categoryID);

        public Task<Category?> GetEntity(string name);

        public Category Create(string name, byte[] photo);

        public Task<Category?> Modify(int categoryID, string name, byte[] photo);

        public Task Delete(int categoryID);

        #endregion

    }
}