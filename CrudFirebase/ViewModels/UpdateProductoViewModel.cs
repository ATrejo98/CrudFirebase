using System.Windows.Input;
using CrudFirebase.Models;
using CrudFirebase.Controllers;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace CrudFirebase.ViewModels
{
    public class UpdateProductoViewModel : BaseViewModel
    {
        private readonly FirebaseService _firebaseService;
        private productosModel _producto;

        public productosModel Producto
        {
            get => _producto;
            set => SetProperty(ref _producto, value);
        }

        public ICommand UpdateProductoCommand { get; }

        public UpdateProductoViewModel(productosModel producto)
        {
            _firebaseService = new FirebaseService();
            Producto = producto;

            UpdateProductoCommand = new Command(async () => await UpdateProducto());
        }

        private async Task UpdateProducto()
        {
            await _firebaseService.UpdateProducto(Producto);
            // Aquí puedes agregar lógica adicional después de actualizar, como navegar a otra página
        }
    }
}
