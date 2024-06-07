using CommunityToolkit.Maui.Views;
using Room_Scheduling_Software.Data;
using Room_Scheduling_Software.Data.Entities;
using Room_Scheduling_Software.Views;
using System.Diagnostics;

namespace Room_Scheduling_Software.Views;

public partial class NewAppointment : Popup
{
	Room? _room = null;
	DateTime dateNow;
	DateTime dateEnd;
	Appointment _appointment;

	bool isUpdate = false;
	int idAp_Update = -1;

	Page _page;

	public NewAppointment(Page page, int id, Appointment _ap = null)
	{
		InitializeComponent();

		_page = page;
		_appointment = new Appointment();

		// Get Room Data
		using (var db = new Context())
		{
			_room = db.Rooms
				.Where(r => r.Id == id)
				.First();
		}

		// CHECK -> Room is OCCUPIED
		if (_ap != null)
		{
			// Update variables and info
			isUpdate = true;
			idAp_Update = _ap.Id;

			Title.Text = "CHECK APPOINTMENT";
			CreateButton.Text = "Update";

            using (var db = new Context())
            {
				// Get User Data
				var u = db.Users
					.Where(u => u.Id == _ap.UserId)
					.First();

				User_Name.Text = u.Name;
				User_Email.Text = u.Email;
				User_Visits.Text = u.Number_Visits.ToString();

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
		using (var db= new Context())
		{
			User? _user = db.Users
				.Where(u => u.Email == User_Email.Text)
				.FirstOrDefault();
			
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
		}

		ErrorMessage("");
	}

	private void ErrorMessage(string message)
	{
		Error_Message.Text = message;
	}

	private void Create(object sender, EventArgs e)
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
        using (var db = new Context())
		{
			User? _user = db.Users
				.Where(u => u.Email == User_Email.Text)
				.FirstOrDefault();

			Room? roomTemp = db.Rooms
				.Where(r => r.Id == _room.Id)
				.First();

            if (_user != null)
            {
                _appointment.UserId = _user.Id;
				_appointment.RoomId = roomTemp.Id;
				_appointment.Start_Hour = dateNow;
				_appointment.End_Hour = dateEnd;

                if (isUpdate)
                {
                    var ap = db.Appointments
                        .Where(a => a.Id == idAp_Update)
                        .First();

					if (ap.UserId != _appointment.UserId)
					{
						ap.UserId = _appointment.UserId;
                        _user.Number_Visits += 1;
                    }

					ap.End_Hour = dateEnd;
					ap.Price = _appointment.Price;
                }
				else
				{
					db.Add( _appointment );
					roomTemp.IsFree = false;
					_user.Number_Visits += 1;
				}

				db.SaveChanges();

				Close(_appointment);
            }
			ErrorMessage("SOMETHING WENT WRONG");
        }
	}
}