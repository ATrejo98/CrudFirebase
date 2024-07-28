using Microsoft.Maui.Controls;
using CrudFirebase.ViewModels;

namespace CrudFirebase.Views
{
    public partial class ProductosPage : ContentPage
    {
        public ProductosPage()
        {
            InitializeComponent();
            BindingContext = new ProductosViewModel();
        }

        private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {

        }
    }
}
