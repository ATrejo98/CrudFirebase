using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrudFirebase.Models;
using Firebase.Database.Query;

namespace CrudFirebase.Controllers
{
    internal class FirebaseService
    {

        private readonly FirebaseClient _firebaseClient;

        public FirebaseService()
        {
            _firebaseClient = new FirebaseClient("https://crudnetmaui-default-rtdb.firebaseio.com/");
        }

        //Agregar Porducto

        public async Task AddProducto(productosModel producto)
        {
            await _firebaseClient
                .Child("Productos")
                .PostAsync(producto);
        }

        //Leer productos

        public async Task<List<productosModel>> GetProductos()
        {
            return (await _firebaseClient
                .Child("Productos")
                .OnceAsync<productosModel>()).Select(item => new productosModel
                {
                    Id = item.Key,
                    Nombre = item.Object.Nombre,
                    Descripcion = item.Object.Descripcion,
                    Precio = item.Object.Precio,
                    Foto = item.Object.Foto
                }).ToList();
        }

        //actualizar productos
        public async Task UpdateProducto(productosModel producto)
        {
            await _firebaseClient
                .Child("Productos")
                .Child(producto.Id)
                .PutAsync(producto);
        }

        //eliminar porductos

        public async Task DeleteProducto(string id)
        {
            await _firebaseClient
                .Child("Productos")
                .Child(id)
                .DeleteAsync();
        }


    }
}
