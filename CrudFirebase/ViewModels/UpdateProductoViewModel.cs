using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CrudFirebase.Models;
using CrudFirebase.Controllers;

namespace CrudFirebase.ViewModels
{
    public class UpdateProductoViewModel : INotifyPropertyChanged
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
            // Acumular mensajes de error para campos vacíos
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(Producto.Nombre))
            {
                errores.Add("El campo 'Nombre' está vacío.");
            }

            if (string.IsNullOrWhiteSpace(Producto.Descripcion))
            {
                errores.Add("El campo 'Descripción' está vacío.");
            }

            if (string.IsNullOrWhiteSpace(Producto.Precio))
            {
                errores.Add("El campo 'Precio' está vacío.");
            }

            if (string.IsNullOrWhiteSpace(Producto.FotoBase64))
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

            // Confirmación de modificación
            bool isConfirmed = await Application.Current.MainPage.DisplayAlert("Confirmar modificación", "¿Estás seguro de que deseas modificar los elementos de este producto?", "Sí", "No");
            if (isConfirmed)
            {
                await _firebaseService.UpdateProducto(Producto);
                await Application.Current.MainPage.Navigation.PopAsync();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }

    }
}
