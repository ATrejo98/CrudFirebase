using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using CrudFirebase.Controllers;
using CrudFirebase.Models;

namespace CrudFirebase.ViewModels
{
    public class ListProductoViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<productosModel> _productos;
        private ObservableCollection<productosModel> _productosFiltrados;
        private string _searchQuery;
        private readonly FirebaseService _firebaseService;

        public ObservableCollection<productosModel> Productos
        {
            get => _productos;
            set
            {
                _productos = value;
                OnPropertyChanged(nameof(Productos));
            }
        }

        public ObservableCollection<productosModel> ProductosFiltrados
        {
            get => _productosFiltrados;
            set
            {
                _productosFiltrados = value;
                OnPropertyChanged(nameof(ProductosFiltrados));
            }
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
            }
        }

        public ICommand SearchCommand { get; }

        public ListProductoViewModel()
        {
            _firebaseService = new FirebaseService();
            Productos = new ObservableCollection<productosModel>();
            ProductosFiltrados = new ObservableCollection<productosModel>();
            LoadProducts();
        }

        private async void LoadProducts()
        {
            try
            {
                var productos = await _firebaseService.GetProductos();
                foreach (var producto in productos)
                {
                    Productos.Add(producto);
                }
                ProductosFiltrados = new ObservableCollection<productosModel>(Productos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar productos: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
