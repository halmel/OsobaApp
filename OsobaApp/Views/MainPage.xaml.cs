using Microsoft.Maui.Controls;
using OsobaApp.ViewModels;

namespace OsobaApp.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(OsobaViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
           
        }
    }
}
