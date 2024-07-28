using System.Collections.ObjectModel;
using System.Windows.Input;
using CrudFirebase.Models;
using CrudFirebase.Controllers;
using SkiaSharp;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;


namespace CrudFirebase.ViewModels
{
    public class ProductosViewModel : BaseViewModel
    {
        private readonly FirebaseService _firebaseService;
        private productosModel _nuevoProducto;
        private string _uriFoto;

        public productosModel NuevoProducto
        {
            get => _nuevoProducto;
            set => SetProperty(ref _nuevoProducto, value);
        }

        public string UriFoto
        {
            get => _uriFoto;
            set => SetProperty(ref _uriFoto, value);
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

            // Cargar productos al inicializar
            LoadProductosCommand.Execute(null);
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
            LimpiarCampos(); // Llamada al mtodo para limpiar los campos
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

        private void LimpiarCampos()
        {
            NuevoProducto = new productosModel(); // Limpiar campos del modelo de producto
            UriFoto = string.Empty; // Limpiar el URI de la foto
            OnPropertyChanged(nameof(NuevoProducto));
            OnPropertyChanged(nameof(UriFoto));
        }

    }
}
