using CommunityToolkit.Maui.Views;
using Room_Scheduling_Software.Data.Entities;
using Room_Scheduling_Software.Data.Repositories;

namespace Room_Scheduling_Software.Views
{
    public partial class NewCategory : Popup
    {
        #region < DATA MEMBERS >

        public MemoryStream? _Mstream;
        private Category? categoryU = null;

        #endregion

        #region < CONSTRUCTORS >

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

        #endregion

        #region < UI EVENTS >

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

        private async void Create(object sender, EventArgs e) 
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

            // Modify Category
            if (categoryU != null)
                categoryU = await CategoryRepository.GetInstance().Modify(categoryU.Id, Category_Name.Text, _Mstream?.ToArray());
            // Create new Category
            else
                categoryU = CategoryRepository.GetInstance().Create(Category_Name.Text, _Mstream.ToArray());

            Close(categoryU);
        }

        #endregion

    }
}