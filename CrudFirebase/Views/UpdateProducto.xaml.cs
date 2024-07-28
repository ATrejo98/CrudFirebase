using Microsoft.Maui.Controls;
using CrudFirebase.ViewModels;
using CrudFirebase.Models;

namespace CrudFirebase.Views
{
    public partial class UpdateProducto : ContentPage
    {
        public UpdateProducto(productosModel producto)
        {
            InitializeComponent();
            BindingContext = new UpdateProductoViewModel(producto);
        }

        private async void btnfoto_Clicked(object sender, EventArgs e)
        {
            var cameraPermission = await CheckAndRequestPermission<Permissions.Camera>();
            var storageReadPermission = await CheckAndRequestPermission<Permissions.StorageRead>();
            var storageWritePermission = await CheckAndRequestPermission<Permissions.StorageWrite>();

            if (!cameraPermission)
            {
                await DisplayAlert("Permiso denegado", "Se requiere permiso de c�mara para tomar fotos.", "OK");
                return;
            }

            if (!storageReadPermission || !storageWritePermission)
            {
                await DisplayAlert("Permiso denegado", "Se requiere permiso de almacenamiento para guardar fotos.", "OK");
                return;
            }

            var action = await DisplayActionSheet("Seleccionar opci�n", "Cancelar", null, "Tomar foto", "Elegir de la galer�a");

            if (action == "Tomar foto")
            {
                await OpenCamera();
            }
            else if (action == "Elegir de la galer�a")
            {
                await OpenGallery();
            }
        }

        private async Task<bool> CheckAndRequestPermission<TPermission>() where TPermission : Permissions.BasePermission, new()
        {
            var status = await Permissions.CheckStatusAsync<TPermission>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<TPermission>();
            }
            return status == PermissionStatus.Granted;
        }

        private async Task OpenCamera()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();

                if (photo == null)
                {
                    return;
                }

                using (var stream = await photo.OpenReadAsync())
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    var photoBytes = memoryStream.ToArray();

                    var photoBase64 = Convert.ToBase64String(photoBytes);

                    var viewModel = BindingContext as UpdateProductoViewModel;
                    if (viewModel != null)
                    {
                        viewModel.Producto.FotoBase64 = photoBase64;
                    }
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Error", "La caracter�stica no es compatible en este dispositivo.", "OK");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Permiso denegado", "Se requieren permisos de c�mara y almacenamiento para tomar fotos.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ocurri� un error al intentar capturar la foto.", "OK");
            }
        }

        private async Task OpenGallery()
        {
            try
            {
                var photo = await MediaPicker.PickPhotoAsync();

                if (photo == null)
                {
                    return;
                }

                using (var stream = await photo.OpenReadAsync())
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    var photoBytes = memoryStream.ToArray();

                    var photoBase64 = Convert.ToBase64String(photoBytes);

                    var viewModel = BindingContext as UpdateProductoViewModel;
                    if (viewModel != null)
                    {
                        viewModel.Producto.FotoBase64 = photoBase64;
                    }
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Error", "La caracter�stica no es compatible en este dispositivo.", "OK");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Permiso denegado", "Se requieren permisos de c�mara y almacenamiento para elegir fotos.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ocurri� un error al intentar seleccionar la foto.", "OK");
            }
        }

    }
}
