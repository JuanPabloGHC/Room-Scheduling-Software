using CommunityToolkit.Maui.Views;
using Room_Scheduling_Software.Views;
using Room_Scheduling_Software.Data.Entities;
using Room_Scheduling_Software.Data.Repositories;
using System.Collections.ObjectModel;

namespace Room_Scheduling_Software.Pages;

public partial class Admin : ContentPage
{
	public ObservableCollection<Category> Categories_collection;
	public ObservableCollection<Room> Rooms_collection;
	public Admin()
	{
		InitializeComponent();

        Categories_collection = new ObservableCollection<Category>();
		Rooms_collection = new ObservableCollection<Room>();

        LoadInformation();
	}

	private async Task LoadInformation()
	{
		// Get Categories
		Categories_collection.Clear();

		List<Category> categoriesList = await RepositoryController.GetInstance().Categories.GetAll();

		foreach (var c in categoriesList)
		{
			Categories_collection.Add(c);
		}

		// Get Rooms
		Rooms_collection.Clear();

		List<Room> roomsList = await RepositoryController.GetInstance().Rooms.GetAll();

		foreach (var r in roomsList)
		{
			Rooms_collection.Add(r);
		}

		// Update VerticalStackLayout of Rooms
		ContainerRooms.Children.Clear();

		foreach (var ro in Rooms_collection)
		{
			Stream stream  = new MemoryStream(ro.Category.Photo);

			// BUTTONS Delete - Update
            Button buttonD = new Button
            {
                Text = "X",
                TextColor = Colors.Red,
                BackgroundColor = Colors.Transparent,
                BorderColor = Colors.Red,
                BorderWidth = 3,
                CornerRadius = 3,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                AutomationId = ro.Id.ToString()
            };
            buttonD.Clicked += DeleteRoom;

            Button buttonU = new Button
            {
                Text = "i",
                TextColor = Colors.Orange,
                BackgroundColor = Colors.Transparent,
                BorderColor = Colors.Orange,
                BorderWidth = 3,
                CornerRadius = 3,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                AutomationId = ro.Id.ToString()
            };
            buttonU.Clicked += AddRoom;

			Grid gridButtons = new Grid
			{
				ColumnDefinitions =
				{
					new ColumnDefinition { Width = GridLength.Star },
					new ColumnDefinition { Width = GridLength.Star }
				},
				ColumnSpacing = 3
			};
			gridButtons.Add(buttonU, column: 0);
			gridButtons.Add(buttonD, column: 1);

			// GRID
            Grid grid = new Grid
			{
				ColumnDefinitions =
				{
					new ColumnDefinition { Width = 100 },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = 40 },
                    new ColumnDefinition { Width = 100 },
                    new ColumnDefinition { Width = GridLength.Auto }
                }
			};
            
			// Image row 0 column 0
			grid.Add(new Image
            {
                Source = ImageSource.FromStream(() => stream),
                WidthRequest = 100,
                HeightRequest = 100,
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            }, column: 0);
            
			// Name row 0 column 1
			grid.Add(new Label
            {
                Text = ro.Name,
                TextColor = Color.FromArgb("#6e28c3"),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            }, column: 1);
			
			// Capacity row 0 column 2
			string capacity = "# " + ro.Capacity.ToString();
            grid.Add(new Label
            {
                Text = capacity,
                TextColor = Color.FromArgb("#6e28c3"),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            }, column: 2);
            
			// Price row 0 column 3
			string price = "$ " + ro.Hourly_Price.ToString();
            grid.Add(new Label
            {
                Text = price,
                TextColor = Color.FromArgb("#6e28c3"),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            }, column: 3);
			
			// Buttons row 0 column 4
			grid.Add(gridButtons, column: 4);

			// FRAME
            Frame frame = new Frame
            {
                CornerRadius = 5,
                BackgroundColor = Color.FromArgb("#f1e9fe"),
                Padding = new Thickness(10),
                Margin = new Thickness(5),
                Content = grid
            };

            ContainerRooms.Add(frame);

        }

		// Update VerticalStackLayout of Categories
		ContainerCategories.Children.Clear();

		foreach (var ca in Categories_collection)
		{
			Stream stream = new MemoryStream(ca.Photo);

            // BUTTONS Delete - Update
            Button buttonD = new Button 
			{
                Text = "X",
                TextColor = Colors.Red,
                BackgroundColor = Colors.Transparent,
                BorderColor = Colors.Red,
                BorderWidth = 3,
                CornerRadius = 3,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
				AutomationId = ca.Id.ToString()
            };
			buttonD.Clicked += DeleteCategory;

            Button buttonU = new Button
            {
                Text = "i",
                TextColor = Colors.Orange,
                BackgroundColor = Colors.Transparent,
                BorderColor = Colors.Orange,
                BorderWidth = 3,
                CornerRadius = 3,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                AutomationId = ca.Id.ToString()
            };
            buttonU.Clicked += AddCategory;

            Grid gridButtons = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star }
                },
                ColumnSpacing = 3
            };
            gridButtons.Add(buttonU, column: 0);
            gridButtons.Add(buttonD, column: 1);

			// GRID
            Grid grid = new Grid
			{
				ColumnDefinitions =
				{
                    new ColumnDefinition { Width = 100 },
                    new ColumnDefinition { Width = GridLength.Star },
					new ColumnDefinition { Width = GridLength.Auto }
                }
			};
			
			// Image row 0 column 0
			grid.Add(new Image
			{
                Source = ImageSource.FromStream(() => stream),
                WidthRequest = 100,
                HeightRequest = 100,
                Aspect = Aspect.AspectFit,
				HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            }, column: 0);
			
			// Name row 0 column 1
			grid.Add(new Label
			{
                Text = ca.Name,
                TextColor = Color.FromArgb("#6e28c3"),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            }, column: 1);
			
			// Buttons row 0 column 2
			grid.Add(gridButtons, column: 2);

			// FRAME
			Frame frame = new Frame
			{
				CornerRadius = 5,
				BackgroundColor = Color.FromArgb("#f1e9fe"),
				Padding = new Thickness(10),
				Margin = new Thickness(5),
				Content = grid
			};

			ContainerCategories.Add(frame);
		}

		Home.LoadInformation();
	}

	private async void AddCategory(object? sender, EventArgs e)
	{
		Category? _category = null;

		Button? _button = sender as Button;

		// Update category
		if (_button?.AutomationId != null)
		{
			_category = await RepositoryController.GetInstance().Categories.GetEntity(Convert.ToInt32(_button.AutomationId));
		}

		var popup = new NewCategory(_category);

		var result = await this.ShowPopupAsync(popup);

		if (result != null)
		{
			await LoadInformation();
		}
	}

	private async void DeleteCategory(object? sender, EventArgs e)
	{
		Button? _b = sender as Button;
		int id = Convert.ToInt32(_b?.AutomationId);

		// Delete Category in DB
		await RepositoryController.GetInstance().Categories.Delete(id);

        await LoadInformation();
    }

	private async void AddRoom(object? sender, EventArgs e)
	{
		Room? _room = null;

		Button? _button = sender as Button;

		// Update Room
		if (_button?.AutomationId != null)
		{
			_room = await RepositoryController.GetInstance().Rooms.GetEntity(Convert.ToInt32(_button.AutomationId));
		}

		var popup = new NewRoom(_room);

		var result = await this.ShowPopupAsync(popup);

		if (result != null)
		{
			await LoadInformation();
		}
	}

	private async void DeleteRoom(object? sender, EventArgs e)
	{
		Button? _b = sender as Button;
		int id = Convert.ToInt32(_b?.AutomationId);

		// Delete Room in DB
		await RepositoryController.GetInstance().Rooms.Delete(id);
		
		await LoadInformation();
    }

}