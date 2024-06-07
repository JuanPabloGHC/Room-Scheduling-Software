using CommunityToolkit.Maui.Views;
using Room_Scheduling_Software.Data;
using Room_Scheduling_Software.Data.Entities;

namespace Room_Scheduling_Software.Views;

public partial class NewUser : Popup
{
	public NewUser(string email)
	{
		InitializeComponent();

		User_Email.Text = email;
	}

	private void Cancel(object sender, EventArgs e)
	{
		Close();
	}

	private void Create(object sender, EventArgs e)
	{
        // Valid name?
        if (User_Name.Text == null || User_Name.Text == "" || User_Name.Text == " ")
        {
            User_Name.PlaceholderColor = Colors.Red;
            return;
        }

        // Valid email?
        if (User_Email.Text == null || User_Email.Text == "" || User_Email.Text == " ")
        {
            User_Email.PlaceholderColor = Colors.Red;
            return;
        }

        using (var db = new Context())
		{
			User? _user = db.Users
				.Where(u => u.Email == User_Email.Text)
				.FirstOrDefault();

			if (_user == null)
			{
				_user = new User();
				_user.Name = User_Name.Text;
				_user.Email = User_Email.Text;
				_user.Number_Visits = 0;

				db.Users.Add(_user);
				db.SaveChanges();

				Close(_user);
			}

			Error_Message.Text = "ALREADY EXISTS";
		}
	}

}