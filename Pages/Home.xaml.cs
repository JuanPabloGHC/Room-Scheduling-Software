using CommunityToolkit.Maui.Views;
using Room_Scheduling_Software.Views;
using Room_Scheduling_Software.Data.Entities;
using Room_Scheduling_Software.Data.Repositories;
using System.Collections.ObjectModel;

namespace Room_Scheduling_Software.Pages
{
	public partial class Home : ContentPage
	{
		#region < DATA MEMBERS >

		public static ObservableCollection<Room>? Rooms_collection;
		private static VerticalStackLayout Container = new VerticalStackLayout();

		public static Command<int>? BookCommand;

		#endregion

		#region < CONSTRUCTORS >

		public Home()
		{
			InitializeComponent();

			BookCommand = new Command<int>((id) => { Book(id); });

			MainContainer.Children.Add(Container);

			Rooms_collection = new ObservableCollection<Room>();
		
			LoadInformation();
		}

		#endregion

		#region < UI EVENTS >

		public static async Task LoadInformation()
		{
			Container.Children.Clear();
			int columns = 0;
			int rows = 0;
		
			// Get Rooms
			List<Room> roomsList = await RepositoryController.GetInstance().Rooms.GetAll();

			Rooms_collection?.Clear();

			foreach (Room room in roomsList)
			{
				Rooms_collection?.Add(room);
			}
        
			// Update Grid of Rooms
			Grid grid = new Grid
			{
				ColumnDefinitions =
				{
					new ColumnDefinition { Width = GridLength.Star },
					new ColumnDefinition { Width = GridLength.Star }
				},
				RowDefinitions =
				{
					new RowDefinition { Height = GridLength.Star }
				},
				ColumnSpacing = 3,
				RowSpacing = 3
			};
		
			for(int i=0; i<Rooms_collection?.Count(); i++)
			{
				// For each pair of rooms create a new row (1, 2 rooms => 1 row(default), 3, 4 rooms => 2 rows...)
				if (rows == 2)
				{
					grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
					rows = 0;
				}
				rows++;
			}

			columns = 0;
			rows = 0;
			foreach (var room in Rooms_collection)
			{
				Stream stream = new MemoryStream(room?.Category?.Photo);

				// GRID
				Grid gridRoom = new Grid
				{
					ColumnDefinitions =
					{
						new ColumnDefinition { Width= GridLength.Star },
						new ColumnDefinition { Width= GridLength.Star }
					},
					RowDefinitions =
					{
						new RowDefinition { Height = GridLength.Star },
						new RowDefinition { Height = GridLength.Star },
						new RowDefinition { Height = GridLength.Star },
						new RowDefinition { Height = GridLength.Star }
					},
					ColumnSpacing = 3,
					RowSpacing = 3
				};

				// Image row 0 column 0
				gridRoom.Add(new Image
				{
					Source = ImageSource.FromStream(() => stream),
					WidthRequest = 200,
					HeightRequest = 200,
					Aspect = Aspect.AspectFit,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center
				}, row: 0, column: 0);

				// Title row 0 column 1
				gridRoom.Add(new Label
				{
					Text = room.Name,
					FontSize = 30,
					TextColor = Color.FromArgb("#6e28c3"),
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center
				}, row: 0, column: 1);

				// Capacity row 1 column 0
				string capacity = "Limit of " + room.Capacity.ToString() + " people";
				gridRoom.Add(new Label
				{
					Text = capacity,
					TextColor = Color.FromArgb("#6e28c3"),
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center
				}, row: 1, column: 0);

				// Price row 1 column 1
				string price = "$ " + room.Hourly_Price.ToString() + " per hour";
				gridRoom.Add(new Label
				{
					Text = price,
					TextColor = Color.FromArgb("#6e28c3"),
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center
				}, row: 1, column: 1);

				// Status row 2 column 0,1
				Color freeColor = room.IsFree ? Colors.Green : Colors.Red;
				string freeText = room.IsFree ? "FREE" : "OCCUPIED";
				Label freeLabel = new Label
				{
					Text = freeText,
					TextColor = freeColor,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center
				};
				gridRoom.SetRow(freeLabel, 2);
				gridRoom.SetColumnSpan(freeLabel, 2);
				gridRoom.Add(freeLabel);

				// Button row 3 column 0, 1
				string buttonText = room.IsFree ? "BOOK" : "CHECK";
				Color buttonColor = room.IsFree ? Colors.Green : Color.FromArgb("#6e28c3");
				Button button = new Button
				{
					Text = buttonText,
					TextColor = buttonColor,
					BackgroundColor = Colors.Transparent,
					BorderColor = buttonColor,
					BorderWidth = 3,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
					AutomationId = room.Id.ToString()
				};
				button.Command = BookCommand;
				button.CommandParameter = room.Id;
				gridRoom.SetRow(button, 3);
				gridRoom.SetColumnSpan(button, 2);
				gridRoom.Add(button);

				// FRAME
				Frame frame = new Frame
				{
					CornerRadius = 5,
					BackgroundColor = Color.FromArgb("#f1e9fe"),
					Padding = new Thickness(10),
					Margin = new Thickness(5),
					Content = gridRoom
				};

				grid.SetRow(frame, rows);
				grid.SetColumn(frame, columns);
				grid.Add(frame);

				columns += 1;
				if (columns == 2)
				{
					columns = 0;
					rows += 1;
				}

			}

			Container.Children.Add(grid);

		}
	
		private async void Refresh(object sender, EventArgs e)
		{
			await CheckAppointments();
			await LoadInformation();
		}

		#endregion

		#region < PRIVATE METHODS >

		private async void Book(int roomID)
		{
			// Is it OCCUPIED?
			Appointment? current = await RepositoryController.GetInstance().Appointments.GetCurrent(id);
        
			var popup = new NewAppointment(this, roomID, current);

			var result = await this.ShowPopupAsync(popup);

			// Update Page
			if(result != null)
			{
				await LoadInformation();
			}
		}

		private async Task CheckAppointments()
		{
			DateTime now = DateTime.Now;

			// Get the last Appointment of each Room
			List<Appointment?> aps = await RepositoryController.GetInstance().Appointments.GetAllLast();

			// If the time is over -> change the room status' to FREE
			foreach (var ap in aps.Where(ap => ap != null))
			{
				if (ap?.End_Hour < now)
				{
					await RepositoryController.GetInstance().Rooms.ModifyStatus(ap.RoomId, true);
				}
			}
		}

		#endregion

	}
}