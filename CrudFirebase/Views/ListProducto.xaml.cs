using CrudFirebase.Models;
using CrudFirebase.ViewModels;

namespace CrudFirebase.Views;

public partial class ListProducto : ContentPage
{
    public ListProducto()
    {
        InitializeComponent();
        BindingContext = new ProductosViewModel();
    }

    private async void OnDeleteProducto(object sender, EventArgs e)
    {
        var menuItem = sender as MenuItem;
        var producto = menuItem?.CommandParameter as productosModel;

        if (producto != null)
        {
            bool isConfirmed = await DisplayAlert("Confirmar eliminaci�n", "�Est�s seguro de que deseas eliminar este producto?", "S�", "No");
            if (isConfirmed)
            {
                var viewModel = BindingContext as ProductosViewModel;
                if (viewModel != null)
                {
                    viewModel.DeleteProductoCommand.Execute(producto.Id);
                }
            }
        }
    }

    private async void OnModifyProducto(object sender, EventArgs e)
    {
        var swipeItem = sender as SwipeItem;
        var producto = swipeItem?.CommandParameter as productosModel;

        if (producto != null)
        {
            bool isConfirmed = await DisplayAlert("Confirmar modificaci�n", "�Est�s seguro de que deseas modificar este producto?", "S�", "No");
            if (isConfirmed)
            {
                await Navigation.PushAsync(new UpdateProducto(producto));
            }
        }
    }
}
