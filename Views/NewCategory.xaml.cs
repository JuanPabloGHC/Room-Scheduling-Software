using CommunityToolkit.Maui.Views;
using Room_Scheduling_Software.Data;
using Room_Scheduling_Software.Data.Entities;
using System.Diagnostics;

namespace Room_Scheduling_Software.Views;

public partial class NewCategory : Popup
{
    public MemoryStream? _Mstream;
    private Category? categoryU = null;
	public NewCategory(Category? _category = null)
	{
		InitializeComponent();

        if (_category != null)
        {
            categoryU = _category;

            Category_Name.Text = _category.Name;
            _Mstream = new MemoryStream(_category.Photo);
            SelectedImage.Source = ImageSource.FromStream(() => _Mstream);

            Title.Text = "UPDATE CATEGORY";
            CreateButton.Text = "Update";
        }
	}

    private async void OnPickFileClicked(object sender, EventArgs e)
    {
        try
        {
            // Select a file from your dictory
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select an image"
            });

            if (result != null)
            {
                // Save it as a MemoryStream to keep it opened
                if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                    result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                {
                    var stream = await result.OpenReadAsync();

                    _Mstream = new MemoryStream();

                    await stream.CopyToAsync(_Mstream);
                    _Mstream.Position = 0;
                    
                    // Show it and change the color status of the button
                    SelectedImage.Source = ImageSource.FromStream(() => _Mstream);

                    ImageButton.BorderColor = Colors.Green;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void Cancel(object sender, EventArgs e)
    {
        Close();
    }

    private void Create(object sender, EventArgs e) 
    {

        // Valid name?
        if (Category_Name.Text == null || Category_Name.Text == "" || Category_Name.Text == " ")
        {
            Category_Name.PlaceholderColor = Colors.Red;
            return;
        }
        
        // Valid image?
        if(SelectedImage.Source is null)
        {
            ImageButton.BorderColor = Colors.Red;
            return;
        }

        // Create new Category
        Category _category = new Category();
        _category.Name = Category_Name.Text;
        _category.Photo = _Mstream?.ToArray();

        // MANAGE DB
        using (var db = new Context())
        {    
            // Update Room
            if (categoryU != null)
            {
                var temp_c = db.Categories
                    .Where(c => c.Id == categoryU.Id)
                    .First();

                temp_c.Name = _category.Name;
                temp_c.Photo = _category.Photo;
            }
            // Add the Room to the DB
            else
            {
                db.Add(_category);
            }

            db.SaveChanges();
        }

        Close(_category);
    }

}