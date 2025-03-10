using Microsoft.EntityFrameworkCore;
using Room_Scheduling_Software.Data.Entities;
using Room_Scheduling_Software.Data.Interfaces;

namespace Room_Scheduling_Software.Data.Repositories
{
    public class CategoryRepository: ICategoryRepository
    {
        #region < DATA MEMBERS >

        private static CategoryRepository instance;

        #endregion

        #region < CONSTRUCTORS >

        private CategoryRepository() { }

        #endregion

        #region < PATTERN IMPLEMENTATIONS >

        #region < SINGLETON >

        public static CategoryRepository GetInstance()
        {
            if (instance == null)
                instance = new CategoryRepository();

            return instance;
        }

        #endregion

        #endregion

        #region < PUBLIC METHODS >

        public async Task<List<Category>> GetAll()
        {
            using (var db = new Context())
            {
                return await db.Categories
                    .ToListAsync();
            }
        }

        public async Task<Category?> GetEntity(int categoryID)
        {
            using (var db = new Context())
            {
                return await db.Categories
                    .Where(c => c.Id == categoryID)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<Category?> GetEntity(string name)
        {
            using (var db = new Context())
            {
                return await db.Categories
                    .Where(c => c.Name == name)
                    .FirstOrDefaultAsync();
            }
        }

        public Category Create(string name, byte[] photo)
        {
            using (var db = new Context())
            {
                Category category = new Category(name, photo);

                db.Add(category);

                db.SaveChanges();

                return category;
            }
        }

        public async Task<Category?> Modify(int categoryID, string name, byte[] photo)
        {
            using (var db = new Context())
            {
                Category? category = await this.GetEntity(categoryID);

                if (category == null)
                    return null;

                category.Name = name;
                category.Photo = photo;

                db.SaveChanges();

                return category;
            }
        }

        public async Task Delete(int categoryID)
        {
            Category? category = await this.GetEntity(categoryID);

            if (category == null)
                return;

            using (var db = new Context())
            {
                db.Remove(category);

                await db.SaveChangesAsync();
            }
        }

        #endregion

    }
}