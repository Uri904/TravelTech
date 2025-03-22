using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelTech.Tablas;
using Xamarin.Forms;
using TravelTech.Views.Viajes;
using SQLite;
using System.IO;
using SQLiteNetExtensions.Extensions;

namespace TravelTech
{
    public partial class MainPage : ContentPage
    {
        // Ruta a la base de datos SQLite
        private string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TravelTech.db3");

        public MainPage()
        {
            InitializeComponent();

            // Crear la base de datos si no existe
            using (var db = new SQLiteConnection(dbPath))
            {
                db.CreateTable<T_Viaje>();
                db.CreateTable<T_Destino>();
                db.CreateTable<T_Actividad>();
                db.CreateTable<T_Gasto>();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CargarViajes();
        }

        // Método para cargar todos los viajes desde la base de datos
        private void CargarViajes()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                {
                    // Obtener todos los viajes
                    var viajes = conn.Table<T_Viaje>().ToList();

                    // Crear una lista con la información necesaria para mostrar
                    var viajesDisplay = new List<ViajeDisplayModel>();

                    foreach (var viaje in viajes)
                    {
                        // Obtener actividades relacionadas para el resumen
                        var actividades = conn.Table<T_Actividad>().Where(a => a.PK_id_viaje == viaje.Id).ToList();
                        string actividadesResumen = actividades.Count > 0
                            ? $"{actividades.Count} actividades"
                            : "Sin actividades";

                        viajesDisplay.Add(new ViajeDisplayModel
                        {
                            Id = viaje.Id,
                            nombre = viaje.nombre,
                            Fecha_inicio = viaje.Fecha_inicio,
                            Presupuesto = $"${viaje.Presupuesto}",
                            ActividadesResumen = actividadesResumen
                        });
                    }

                    // Asignar a ListView
                    listaViajes.ItemsSource = viajesDisplay;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Error al cargar viajes: {ex.Message}", "OK");
            }
        }

        // Clase auxiliar para mostrar
        public class ViajeDisplayModel
        {
            public int Id { get; set; }
            public string nombre { get; set; }
            public string Fecha_inicio { get; set; }
            public string Presupuesto { get; set; }
            public string ActividadesResumen { get; set; }
        }

        // Manejadores de eventos de botones
        private void btn_VerDetalles(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var viajeId = (int)button.CommandParameter;

            // Navegar a la página de detalles pasando el ID del viaje
            Navigation.PushAsync(new Detalles(viajeId));
        }

        private void btn_CrearViaje(object sender, EventArgs e)
        {
            // Navegar a la página de crear viaje
            Navigation.PushAsync(new Crear());
        }

        private void btn_EliminarViaje(object sender, EventArgs e)
        {
            var imageButton = (ImageButton)sender;
            var viajeId = (int)imageButton.CommandParameter;  // Obtenemos el Id del viaje desde el CommandParameter

            if (viajeId <= 0)
            {
                DisplayAlert("Error", "El Id del viaje no es válido", "OK");
                return;
            }

            // Confirmar y eliminar
            Device.BeginInvokeOnMainThread(async () =>
            {
                bool confirmar = await DisplayAlert("Confirmar", "¿Estás seguro de eliminar este viaje?", "Sí", "No");
                if (confirmar)
                {
                    try
                    {
                        using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                        {
                            // Eliminar los registros relacionados
                            conn.Execute("DELETE FROM T_Actividad WHERE PK_id_viaje = ?", viajeId);
                            conn.Execute("DELETE FROM T_Gasto WHERE PK_id_viaje = ?", viajeId);
                            conn.Execute("DELETE FROM T_Destino WHERE PK_id_viaje = ?", viajeId);

                            // Eliminar el viaje
                            conn.Delete<T_Viaje>(viajeId);
                        }

                        // Recargar viajes
                        CargarViajes();
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", $"No se pudo eliminar el viaje: {ex.Message}", "OK");
                    }
                }
            });
        }



        private void btn_Calendario(object sender, EventArgs e)
        {

            Navigation.PushAsync(new Views.CalendarioIn());
            // Implementación después
        }

        private void btn_AbrirMenu(object sender, EventArgs e)
        {
            // Mostrar el menú lateral
            (Application.Current.MainPage as FlyoutPage).IsPresented = true;
        }
    }
}