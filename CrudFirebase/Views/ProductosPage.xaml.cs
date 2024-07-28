using Microsoft.Maui.Controls;
using CrudFirebase.ViewModels;
using System.Diagnostics;
using CrudFirebase.Controllers;


namespace CrudFirebase.Views
{
    public partial class ProductosPage : ContentPage
    {

        public ProductosPage()
        {
            InitializeComponent();
            BindingContext = new ProductosViewModel();
        }


        private async void NavigateToListPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListProducto());
        }

        private async void btnfoto_Clicked(object sender, EventArgs e)
        {

            var cameraPermission = await CheckAndRequestPermission<Permissions.Camera>();
            var storageReadPermission = await CheckAndRequestPermission<Permissions.StorageRead>();
            var storageWritePermission = await CheckAndRequestPermission<Permissions.StorageWrite>();


            if (!cameraPermission)
            {
                await DisplayAlert("Permiso denegado", "Se requiere permiso de cámara para tomar fotos.", "OK");
                return;
            }

            if (!storageReadPermission || !storageWritePermission)
            {
                await DisplayAlert("Permiso denegado", "Se requiere permiso de almacenamiento para guardar fotos.", "OK");
                return;
            }

            // Mostrar opciones de cámara o galería
            var action = await DisplayActionSheet("Seleccionar opción", "Cancelar", null, "Tomar foto", "Elegir de la galería");

            if (action == "Tomar foto")
            {
                await OpenCamera();
            }
            else if (action == "Elegir de la galería")
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
                // Intenta capturar una foto usando MediaPicker
                var photo = await MediaPicker.CapturePhotoAsync();

                // Verifica si la foto fue capturada correctamente
                if (photo == null)
                {
                    // Si la captura de la foto fue cancelada, salir del método
                    return;
                }

                // Obtener el URI de la foto
                var photoUri = photo.FullPath;

                // Convertir la foto a base64
                using (var stream = await photo.OpenReadAsync())
                using (var memoryStream = new MemoryStream())
                {
                    // Copia el contenido del stream de la foto al memoryStream
                    await stream.CopyToAsync(memoryStream);
                    var photoBytes = memoryStream.ToArray();

                    // Convierte el array de bytes de la foto a una cadena base64
                    var photoBase64 = Convert.ToBase64String(photoBytes);

                    // Asignar la foto en base64 al modelo de producto
                    var viewModel = BindingContext as ProductosViewModel;
                    if (viewModel != null)
                    {
                        viewModel.NuevoProducto.FotoBase64 = photoBase64;
                        viewModel.UriFoto = photoUri; // Asignar el URI de la foto
                    }
                }

            }

            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Error", "La característica no es compatible en este dispositivo.", "OK");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Permiso denegado", "Se requieren permisos de cámara y almacenamiento para tomar fotos.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ocurrió un error al intentar capturar la foto.", "OK");
            }
        }

        private async Task OpenGallery()
        {
            try
            {
                // Intentar seleccionar una foto de la galería usando MediaPicker
                var photo = await MediaPicker.PickPhotoAsync();

                // Verificar si la foto fue seleccionada correctamente
                if (photo == null)
                {
                    // Si la selección de la foto fue cancelada, salir del método
                    return;
                }

                // Obtener el URI de la foto
                var photoUri = photo.FullPath;

                // Convertir la foto a base64
                using (var stream = await photo.OpenReadAsync())
                using (var memoryStream = new MemoryStream())
                {
                    // Copiar el contenido del stream de la foto al memoryStream
                    await stream.CopyToAsync(memoryStream);
                    var photoBytes = memoryStream.ToArray();

                    // Convertir el array de bytes de la foto a una cadena base64
                    var photoBase64 = Convert.ToBase64String(photoBytes);

                    // Asignar la foto en base64 al modelo de producto
                    var viewModel = BindingContext as ProductosViewModel;
                    if (viewModel != null)
                    {
                        viewModel.NuevoProducto.FotoBase64 = photoBase64;
                        viewModel.UriFoto = photoUri; // Asignar el URI de la foto
                    }
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Error", "La característica no es compatible en este dispositivo.", "OK");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Permiso denegado", "Se requieren permisos de cámara y almacenamiento para elegir fotos.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ocurrió un error al intentar seleccionar la foto.", "OK");
            }
        }


    }
}
