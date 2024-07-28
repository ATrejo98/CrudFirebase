using CrudFirebase.Models;
using CrudFirebase.ViewModels;

namespace CrudFirebase.Views;

public partial class ListProducto : ContentPage
{
    public ListProducto()
    {
        InitializeComponent();
        BindingContext = new ProductosViewModel(); // Aseg�rate de que el ViewModel est� establecido como BindingContext
    }

    private async void OnDeleteProducto(object sender, EventArgs e)
    {
        var menuItem = sender as MenuItem;
        var producto = menuItem?.CommandParameter as productosModel;

        if (producto != null)
        {
            var viewModel = BindingContext as ProductosViewModel;
            if (viewModel != null)
            {
                viewModel.DeleteProductoCommand.Execute(producto.Id);
            }
        }
    }
}
