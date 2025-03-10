using CommunityToolkit.Maui.Views;
using Room_Scheduling_Software.Data.Entities;
using Room_Scheduling_Software.Data.Repositories;

namespace Room_Scheduling_Software.Views;

public partial class NewAppointment : Popup
{
    Room? _room = null;
	DateTime dateNow;
	DateTime dateEnd;
	Appointment? _appointment;

	bool isUpdate = false;
	int idAp_Update = -1;

	Page _page;

	public NewAppointment(Page page, int roomID, Appointment? _ap = null)
	{
		InitializeComponent();

		_page = page;
		_appointment = new Appointment();

		this.GetData(roomID, _ap);

    }

	private async void GetData(int roomID, Appointment? _ap)
	{
        // Get Room Data
        _room = await RepositoryController.GetInstance().Rooms.GetEntity(roomID);

        // CHECK -> Room is OCCUPIED
        if (_ap != null)
        {
            // Update variables and info
            isUpdate = true;
            idAp_Update = _ap.Id;

            Title.Text = "CHECK APPOINTMENT";
            CreateButton.Text = "Update";

            // Get User Data
            User_Name.Text = _ap?.User?.Name;
            User_Email.Text = _ap?.User?.Email;
            User_Visits.Text = _ap?.User?.Number_Visits.ToString();

            // Get Appointment Data
            dateNow = _ap.Start_Hour.Value;
            Hour_Start.Time = new TimeSpan(dateNow.Hour, dateNow.Minute, 0);

            if (_ap.End_Hour != null)
            {
                dateEnd = _ap.End_Hour.Value;
                Hour_End.Time = new TimeSpan(dateEnd.Hour, dateEnd.Minute, 0);
            }

            _appointment.Price = _ap.Price;
            Appointment_Price.Text = "$" + _ap.Price.ToString();
        }
        // BOOK -> Room is FREE
        else
        {
            // Now
            dateNow = DateTime.Now;
            Hour_Start.Time = new TimeSpan(dateNow.Hour, dateNow.Minute, 0);

            dateEnd = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, dateNow.Hour + 1, dateNow.Minute, dateNow.Second);
            Hour_End.Time = new TimeSpan(dateEnd.Hour, dateEnd.Minute, 0);

            // Total price
            // Rest the end hour less the current hour
            TimeSpan temp = Hour_End.Time - dateNow.TimeOfDay;
            // Hour or Fraction Thereof
            int hours = temp.Hours + (temp.Minutes > 5 ? 1 : 0);
            decimal totalPrice = _room.Hourly_Price * (hours);
            Appointment_Price.Text = "$" + totalPrice.ToString();

            _appointment.Price = totalPrice;
        }

        // Price per hour
        Room_Price.Text = "$" + _room.Hourly_Price.ToString();
    }

	private void Cancel(object sender, EventArgs e)
	{
		Close();
	}

	private void OnTimeSelected(object sender, FocusEventArgs e)
	{
		if (e.IsFocused)
		{
            dateEnd = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, Hour_End.Time.Hours, Hour_End.Time.Minutes, 0);
			
			// Valid end_hour?
			if (Hour_End.Time < dateNow.TimeOfDay) 
			{
                Hour_End.BackgroundColor = Color.FromRgba(255, 0, 0, 0.2);
				return;
            }

            Hour_End.BackgroundColor = Colors.Transparent;

            // Total price
            // Rest the end hour less the current hour
            TimeSpan temp = Hour_End.Time - dateNow.TimeOfDay;

            // Hour or Fraction Thereof
            int hours = temp.Hours + (temp.Minutes > 5 ? 1 : 0);

			decimal totalPrice = _room.Hourly_Price * (hours);
            Appointment_Price.Text = "$ " + totalPrice.ToString();

            _appointment.Price = totalPrice;
        }

        ErrorMessage("");
    }

	private async void SearchUser(object sender, EventArgs e)
	{
		User? _user = await RepositoryController.GetInstance().Users.GetEntity(User_Email.Text);

		// FOUND
		if (_user != null)
		{
			User_Name.Text = _user.Name;
			User_Visits.Text = _user.Number_Visits.ToString();
		}
		// NOT FOUND
		else
		{
			var popup = new NewUser(User_Email.Text);

			var result = await PopupExtensions.ShowPopupAsync<NewUser>(_page, popup);

			if (result != null)
			{
				User userTemp = result as User;

				User_Name.Text = userTemp.Name;
				User_Email.Text = userTemp.Email;
				User_Visits.Text = userTemp.Number_Visits.ToString();
			}
        }
		

		ErrorMessage("");
	}

	private void ErrorMessage(string message)
	{
		Error_Message.Text = message;
	}

	private async void Create(object sender, EventArgs e)
	{
		// Valid email?
		if (User_Email.Text == null || User_Email.Text == "" || User_Email.Text == " ")
		{
			User_Email.PlaceholderColor = Colors.Red;
			return;
		}

		// Valid end_hour?
		if (dateNow > dateEnd)
		{
			Hour_End.BackgroundColor = Color.FromRgba(255, 0, 0, 0.2);
			return;
        }

		// SAVE CHANGES
		User? _user = await RepositoryController.GetInstance().Users.GetEntity(User_Email.Text);

		if (_user == null)
            ErrorMessage("SOMETHING WENT WRONG");

        if (isUpdate)
			_appointment = await RepositoryController.GetInstance().Appointments.Modify(idAp_Update, _user.Id, dateEnd, _appointment.Price);
		else
            _appointment = await RepositoryController.GetInstance().Appointments.Create(_room.Id, _user.Id, dateNow, dateEnd, _appointment.Price);

        Close(_appointment);
	}

}