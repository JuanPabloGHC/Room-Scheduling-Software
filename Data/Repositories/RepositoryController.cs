
namespace Room_Scheduling_Software.Data.Repositories
{
    public class RepositoryController
    {
        #region < DATA MEMBERS >

        private static RepositoryController instance;

        #endregion

        #region < PROPERTIES >

        public AppointmentRepository Appointments { get; internal set; }
        public CategoryRepository Categories { get; internal set; }
        public RoomRepository Rooms { get; internal set; }
        public UserRepository Users { get; internal set; }

        #endregion

        #region < CONSTRUCTORS >

        private RepositoryController()
        {
            Appointments = AppointmentRepository.GetInstance();
            Categories = CategoryRepository.GetInstance();
            Rooms = RoomRepository.GetInstance();
            Users = UserRepository.GetInstance();
        }

        #endregion

        #region < PATTERN IMPLEMENTATIONS >

        #region < SINGLETON >

        public static RepositoryController GetInstance()
        {
            if (instance == null)
                instance = new RepositoryController();

            return instance;
        }

        #endregion

        #endregion

    }
}