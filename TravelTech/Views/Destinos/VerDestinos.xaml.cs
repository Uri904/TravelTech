using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using System.IO;
using TravelTech.Tablas;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelTech.Views.Destinos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerDestinos : ContentPage
    {
        private int _destinoId;
        private int _viajeId;
        private string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TravelTech.db3");

        public VerDestinos(int viajeId)
        {
            InitializeComponent();
            _viajeId = viajeId;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            CargarDestinos();
        }
        private void CargarDestinos()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                {
                    // Filtrar destinos por el viajeId actual
                    var destinos = conn.Table<T_Destino>().Where(d => d.PK_id_viaje == _viajeId).ToList();

                    // Crear una lista con la información necesaria para mostrar
                    var destinoDisplay = new List<destinoDisplayModel>();

                    foreach (var destino in destinos)
                    {
                        destinoDisplay.Add(new destinoDisplayModel
                        {
                            Id = destino.Id,
                            nombre = destino.nombre,
                            prioridad = destino.prioridad,
                            nota = destino.nota
                        });
                    }

                    // Asignar a ListView
                    listaViajes.ItemsSource = destinoDisplay;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Error al cargar destinos: {ex.Message}", "OK");
            }
        }

        // Clase auxiliar para mostrar
        public class destinoDisplayModel
        {
            public int Id { get; set; }
            public int PK_id_viaje { get; set; }
            public string nombre { get; set; }
            public string nota { get; set; }
            public string prioridad { get; set; }
        }

        // -- Navegación -- //

        // Evento del Boton Agregar Destino
        private async void btn_AgregarDestino(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Destinos.Crear(_viajeId));
        }

        // Evento del Boton Detalles
        private async void btn_Detalles(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Viajes.Detalles(_viajeId));
        }

        // Evento del Boton Actividades
        private async void btn_Actividades(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ActividadesDestinos.VerActividad());
        }

        // Evento del botón btn_VerGastos
        private async void btn_VerGastos(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Gastos.VerGastos(_viajeId));
        }

        // Evento del btn_Home
        private async void btn_Home(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        // Evento de los botones ToggleParis
        private void ToggleParis(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            // Obtener el StackLayout que contiene el botón y el contentParis
            var parentStack = button.Parent as StackLayout;
            if (parentStack == null) return;

            // Obtener el nombre del destino desde el texto del botón
            string nom = button.Text.Replace(" ▲", "").Replace(" ▼", ""); // Eliminar los símbolos ▲ y ▼ si existen

            // Buscar el StackLayout que representa el contenido oculto
            foreach (var child in parentStack.Children)
            {
                if (child is StackLayout content)
                {
                    // Verificamos que no es el mismo StackLayout que contiene el botón
                    if (content != parentStack)
                    {
                        // Alternar visibilidad
                        content.IsVisible = !content.IsVisible;

                        // Cambiar texto del botón
                        button.Text = content.IsVisible ? $"{nom} ▲" : $"{nom} ▼";
                        break;
                    }
                }
            }
        }

        private async void BtnEditar(object sender, System.EventArgs e)
        {
            // Obtener el destino seleccionado
            var button = (Button)sender;
            var destinoId = (int)button.CommandParameter; // Obtener el destinoId desde el CommandParameter

            // Navegar a ActualizarDestino y pasar el destinoId
            await Navigation.PushAsync(new Views.Destinos.ActualizarDestino(destinoId));
        }

        private void btn_EliminarDestino(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var destinoId = (int)button.CommandParameter;

            // Confirmar y eliminar
            Device.BeginInvokeOnMainThread(async () =>
            {
                bool confirmar = await DisplayAlert("Confirmar", "¿Estás seguro de eliminar este viaje?", "Sí", "No");
                if (confirmar)
                {
                    using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                    {

                        //ELIMINAR el viaje
                        conn.Delete<T_Destino>(destinoId);
                    }

                    // Recargar viajes
                    CargarDestinos();
                }
            });
        }
    }
}