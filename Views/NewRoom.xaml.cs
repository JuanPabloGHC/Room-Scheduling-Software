using CommunityToolkit.Maui.Views;
using Room_Scheduling_Software.Data;
using Room_Scheduling_Software.Data.Entities;
using System.Diagnostics;

namespace Room_Scheduling_Software.Views;

public partial class NewRoom : Popup
{
	public List<string> _categories;
	private Room? roomU = null;
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

	private void LoadCategories()
	{
		int index = -1;
		int count = 0;
		using (var db = new Context())
		{
			foreach (var c in db.Categories.ToList())
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
	}

	private void Cancel(object sender, EventArgs e)
	{
		Close();
	}

	private void  Create(object sender, EventArgs e)
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

		// Create new Room
		Room _room = new Room();
        
		_room.Name = Room_Name.Text;

		_room.Capacity = Convert.ToInt32(Room_Capacity.Text);

		_room.Hourly_Price = Convert.ToDecimal(Room_Price.Text);

		_room.IsFree = true;

		string? sc = Room_Category.SelectedItem as string;
        using (var db = new Context())
		{
			// Searcg the Category in the DB
			int idC = Convert.ToInt32(db.Categories
				.Where(c => c.Name == sc)
				.Select(c => c.Id)
				.First());
			_room.CategoryId = idC;

			// Update Room
			if (roomU != null)
			{
				var temp_r = db.Rooms
					.Where(r => r.Id == roomU.Id)
					.First();

				temp_r.Name = _room.Name;
				temp_r.Capacity = _room.Capacity;
				temp_r.Hourly_Price = _room.Hourly_Price;
				temp_r.CategoryId = _room.CategoryId;
			}
			// Add the Room to the DB
			else
			{
				db.Add(_room);
			}

			db.SaveChanges();
		}

        Close(_room);
	}
}