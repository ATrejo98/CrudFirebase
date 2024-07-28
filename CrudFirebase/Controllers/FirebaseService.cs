using Firebase.Database;
using CrudFirebase.Models;
using Firebase.Database.Query;

namespace CrudFirebase.Controllers
{
    internal class FirebaseService
    {
        private readonly FirebaseClient _firebaseClient;

        public FirebaseService()
        {
            _firebaseClient = new FirebaseClient("https://fir-netmauicrud-default-rtdb.firebaseio.com/");
        }

        // Agregar Producto
        public async Task AddProducto(productosModel producto)
        {
            await _firebaseClient
                .Child("Productos")
                .PostAsync(producto);
        }

        // Leer productos
        public async Task<List<productosModel>> GetProductos()
        {
            return (await _firebaseClient
                .Child("Productos")
                .OnceAsync<productosModel>()).Select(item => new productosModel
                {
                    Id = item.Key,
                    Nombre = item.Object.Nombre,
                    Descripcion = item.Object.Descripcion,
                    Precio = item.Object.Precio,  // Asegúrate que se maneje como string
                    FotoBase64 = item.Object.FotoBase64
                }).ToList();
        }

        // Actualizar producto
        public async Task UpdateProducto(productosModel producto)
        {
            await _firebaseClient
                .Child("Productos")
                .Child(producto.Id)
                .PutAsync(producto);
        }

        // Eliminar producto
        public async Task DeleteProducto(string id)
        {
            await _firebaseClient
                .Child("Productos")
                .Child(id)
                .DeleteAsync();
        }
    }
}
