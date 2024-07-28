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
            // Acumular mensajes de error para campos vacíos
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(NuevoProducto.Nombre))
            {
                errores.Add("El campo 'Nombre' está vacío.");
            }

            if (string.IsNullOrWhiteSpace(NuevoProducto.Descripcion))
            {
                errores.Add("El campo 'Descripción' está vacío.");
            }

            if (string.IsNullOrWhiteSpace(NuevoProducto.Precio))
            {
                errores.Add("El campo 'Precio' está vacío.");
            }

            if (string.IsNullOrWhiteSpace(NuevoProducto.FotoBase64))
            {
                errores.Add("El campo 'Fotografía' está vacío.");
            }

            // Si hay errores, mostrar alerta y retornar
            if (errores.Any())
            {
                var mensaje = string.Join("\n", errores);
                await Application.Current.MainPage.DisplayAlert("Error", mensaje, "OK");
                return;
            }

            // Confirmación de adición
            bool isConfirmed = await Application.Current.MainPage.DisplayAlert("Confirmar adición", "¿Estás seguro de que deseas añadir este producto?", "Sí", "No");
            if (isConfirmed)
            {
                await _firebaseService.AddProducto(NuevoProducto);
                await LoadProductos();
                LimpiarCampos(); // Llamada al método para limpiar los campos
            }
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
