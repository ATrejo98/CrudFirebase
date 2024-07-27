using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CrudFirebase.Models;
using CrudFirebase.Controllers;
using Microsoft.Maui.Controls;

namespace CrudFirebase.ViewModels
{
    public class ProductosViewModel : BaseViewModel
    {
        private readonly FirebaseService _firebaseService;

        private productosModel _nuevoProducto;
        public productosModel NuevoProducto
        {
            get => _nuevoProducto;
            set => SetProperty(ref _nuevoProducto, value);
        }

        public ObservableCollection<productosModel> Productos { get; private set; }
        public ICommand LoadProductosCommand { get; }
        public ICommand AddProductoCommand { get; }
        public ICommand UpdateProductoCommand { get; }
        public ICommand DeleteProductoCommand { get; }

        public ProductosViewModel()
        {
            _firebaseService = new FirebaseService();
            Productos = new ObservableCollection<productosModel>();
            NuevoProducto = new productosModel();

            LoadProductosCommand = new Command(async () => await LoadProductos());
            AddProductoCommand = new Command(async () => await AddProducto());
            UpdateProductoCommand = new Command<productosModel>(async (producto) => await UpdateProducto(producto));
            DeleteProductoCommand = new Command<string>(async (id) => await DeleteProducto(id));
        }

        private async Task LoadProductos()
        {
            var productos = await _firebaseService.GetProductos();
            Productos.Clear();
            foreach (var producto in productos)
            {
                Productos.Add(producto);
            }
        }

        private async Task AddProducto()
        {
            await _firebaseService.AddProducto(NuevoProducto);
            await LoadProductos();
            NuevoProducto = new productosModel(); // Limpiar campos
            OnPropertyChanged(nameof(NuevoProducto));
        }

        private async Task UpdateProducto(productosModel producto)
        {
            await _firebaseService.UpdateProducto(producto);
            await LoadProductos();
        }

        private async Task DeleteProducto(string id)
        {
            await _firebaseService.DeleteProducto(id);
            await LoadProductos();
        }
    }
}
