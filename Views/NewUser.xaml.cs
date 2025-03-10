using CommunityToolkit.Maui.Views;
using Room_Scheduling_Software.Data.Entities;
using Room_Scheduling_Software.Data.Repositories;

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

	private async void Create(object sender, EventArgs e)
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

		User? _user = await UserRepository.GetInstance().GetEntity(User_Email.Text);

		if (_user != null)
			Error_Message.Text = "ALREADY EXISTS";

		_user = UserRepository.GetInstance().Create(User_Name.Text, User_Email.Text, 0);

		Close(_user);

	}

}