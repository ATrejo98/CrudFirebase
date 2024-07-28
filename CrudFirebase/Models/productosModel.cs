using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CrudFirebase.Models
{
    public class productosModel : INotifyPropertyChanged
    {
        private string _id;
        private string _nombre;
        private string _descripcion;
        private string _precio;
        private string _fotoBase64;

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Nombre
        {
            get => _nombre;
            set => SetProperty(ref _nombre, value);
        }

        public string Descripcion
        {
            get => _descripcion;
            set => SetProperty(ref _descripcion, value);
        }

        public string Precio
        {
            get => _precio;
            set => SetProperty(ref _precio, value);
        }

        public string FotoBase64
        {
            get => _fotoBase64;
            set => SetProperty(ref _fotoBase64, value);
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
