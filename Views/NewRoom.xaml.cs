using CommunityToolkit.Maui.Views;
using Room_Scheduling_Software.Data.Entities;
using Room_Scheduling_Software.Data.Repositories;

namespace Room_Scheduling_Software.Views
{
	public partial class NewRoom : Popup
	{
		#region < DATA MEMBERS >

		public List<string> _categories;
		private Room? roomU = null;

		#endregion

		#region < CONSTRUCTORS >

		public NewRoom(Room? _room = null)
		{
			InitializeComponent();

			if (_room != null)
			{
				roomU = _room;

				Room_Name.Text = _room.Name;
				Room_Capacity.Text = _room.Capacity.ToString();
				Room_Price.Text = _room.Hourly_Price.ToString();

				Title.Text = "UPDATE ROOM";
				CreateButton.Text = "Update";
			}

			_categories = new List<string>();

			LoadCategories();
		}

		#endregion

		#region < UI EVENTS >

		private async void LoadCategories()
		{
			int index = -1;
			int count = 0;

			foreach (var c in await CategoryRepository.GetInstance().GetAll())
			{
				_categories.Add(c.Name);
				if (roomU != null && c.Id == roomU.CategoryId) 
				{
					index = count; 
				}

				count++;
			}

			Room_Category.ItemsSource = _categories;
			Room_Category.SelectedIndex = index;
		}

		private void Cancel(object sender, EventArgs e)
		{
			Close();
		}

		private async void  Create(object sender, EventArgs e)
		{

			// Valid name?
			if (Room_Name.Text == null || Room_Name.Text == "" || Room_Name.Text == " ")
			{
				Room_Name.PlaceholderColor = Colors.Red;
				return;
			}

			// Valid category?
			if (Room_Category.SelectedItem == null)
			{
				Room_Category.BackgroundColor = Color.FromRgba(255, 0, 0, 0.2);
				return;
			}
			else
				Room_Category.BackgroundColor = Color.FromArgb("#dee6ff");

			// Valid capacity?
			if (Room_Capacity.Text == null)
			{
				Room_Capacity.PlaceholderColor = Colors.Red;
				return;
			}
			else if (!int.TryParse(Room_Capacity.Text, out _))
			{
				Room_Capacity.TextColor = Colors.Red;
				return;
			}
			else
				Room_Capacity.TextColor = Colors.Black;

			// Valid price?
			if (Room_Price.Text == null)
			{
				Room_Price.PlaceholderColor = Colors.Red;
				return;
			}
			else if (!decimal.TryParse(Room_Price.Text, out _))
			{
				Room_Price.TextColor = Colors.Red;
				return;
			}
			else
				Room_Price.TextColor = Colors.Black;

			string? sc = Room_Category.SelectedItem as string;

			// Search the Category in the DB
			Category? category = await CategoryRepository.GetInstance().GetEntity(sc);

			if (category == null)
				return;

			// Update Room
			if (roomU != null)
				roomU = await RoomRepository.GetInstance().Modify(roomU.Id, category.Id, Room_Name.Text, Convert.ToInt32(Room_Capacity.Text), Convert.ToDecimal(Room_Price.Text));
			// Create new Room
			else
				roomU = RoomRepository.GetInstance().Create(category.Id, Room_Name.Text, Convert.ToInt32(Room_Capacity.Text), Convert.ToDecimal(Room_Price.Text), true);

			Close(roomU);

		}

		#endregion

	}
}