using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TravelTech.Tablas;
using TravelTech.Views.Viajes;
using SQLite;
using System.IO;
using SQLiteNetExtensions.Extensions;

namespace TravelTech.Views.ActividadesDestinos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerActividad : ContentPage
    {
        private int _viajeId;
        private int actividadId;
        private string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TravelTech.db3");

        public VerActividad(int viajeId)
        {
            InitializeComponent();
            _viajeId = viajeId;

            using (var db = new SQLiteConnection(dbPath))
            {
                db.CreateTable<T_Viaje>();
                db.CreateTable<T_Recordatorio>();
                db.CreateTable<T_Destino>();
                db.CreateTable<T_Actividad>();
                db.CreateTable<T_Gasto>();
            }

        }

        // -- Navegación -- //

        //Evento del botón btn_VerDetalle
        private async void btn_Detalles(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Viajes.Detalles(_viajeId));
        }

        //Evento del botón btn_VerDestino
        private async void btn_VerDestino(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Destinos.VerDestinos(_viajeId));
        }

        //Evento del botón btn_VerGastos
        private async void btn_VerGastos(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Gastos.VerGastos(_viajeId));
        }

        //Evento del botón btn_CrearActividad
        private async void btn_CrearActividad(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ActividadesDestinos.CrearActividad(_viajeId));
        }

        //Evento del botón btn_Home
        private async void btn_Home(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }



        //Evento del botón btn_VerRecordatorios
        private async void btn_VerRecordatorios(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Views.ActividadesDestinos.VerRecordatorios(_viajeId));
        }



        protected override void OnAppearing()
        {
            base.OnAppearing();
            CargarActividad();
        }

        // LISTA DE LAS ACTIVIDADES CREADAS
        private void CargarActividad()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                {

                    var actividades = conn.Table<T_Actividad>().Where(a => a.PK_id_viaje == _viajeId).ToList(); // para obtener todas las ACTIVIDADES

                    var actividadDisplay = new List<ActividadDisplayModel>();  // Lista con la información a mostrar

                    foreach (var actividad in actividades)
                    {


                        actividadDisplay.Add(new ActividadDisplayModel
                        {
                            Id = actividad.Id,
                            nombre = actividad.nombre,
                            Fecha_actividad = actividad.Fecha_actividad,
                            Estado = actividad.Estado,
                            nota = actividad.nota

                        });
                    }


                    listaActividades.ItemsSource = actividadDisplay;// Asignacion a Lista
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Error al cargar viajes: {ex.Message}", "OK");
            }
        }

        // Clase auxiliar para mostrar
        public class ActividadDisplayModel
        {
            public int Id { get; set; }
            public string nombre { get; set; }
            public string Fecha_actividad { get; set; }
            public string nota { get; set; }
            public string Estado { get; set; }
        }

        // editaractividad
        private void btn_EditarActividad(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var actividadId = (int)button.CommandParameter;

            Navigation.PushAsync(new Detalles(actividadId));
        }

        //MOSTRAR ACTIVIDADES
        private void ToggleActividad1(object sender, EventArgs e)
        {

            var button = (Button)sender;

            var viewCell = (ViewCell)button.Parent.Parent.Parent;

            var contentActividad = viewCell.FindByName<StackLayout>("contentActividad1");

            if (contentActividad != null)
            {

                contentActividad.IsVisible = !contentActividad.IsVisible;
                var actividad = (ActividadDisplayModel)viewCell.BindingContext;


                button.Text = contentActividad.IsVisible ? $" ▲" : $" ▼"; // Actualizar botón
            }
            else
            {
                DisplayAlert("Error", "No se encontró el contenido de la actividad.", "OK");
            }
        }

        //Evento de Boton para actiualizar actividad
        private async void btnEditar(object sender, System.EventArgs e)
        {
            var button = (Button)sender;
            var actividadId = (int)button.CommandParameter; // Obtener la actividadId

            await Navigation.PushAsync(new Views.ActividadesDestinos.ActualizarActividad(actividadId));
        }

        //ELIMINAR ACTIVIDAD
        private void btn_EliminarActividad(object sender, EventArgs e)
        {
            var button = (Button)sender;

            // Validar que el CommandParameter sea correcto
            if (button.CommandParameter == null || !(button.CommandParameter is int actividadId))
            {
                DisplayAlert("Error", "No se pudo obtener el ID de la actividad.", "OK");
                return;
            }

            // Confirmar y eliminar
            Device.BeginInvokeOnMainThread(async () =>
            {
                bool confirmar = await DisplayAlert("Confirmar", "¿Está seguro de eliminar la actividad?", "Sí", "No");
                if (confirmar)
                {
                    try
                    {
                        using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                        {
                            conn.Execute("PRAGMA foreign_keys = ON;"); // Asegurar que las llaves foráneas estén activas

                            // Verificar si la actividad existe
                            var actividad = conn.Find<T_Actividad>(actividadId);
                            if (actividad != null)
                            {
                                conn.Delete(actividad, recursive: true);
                                await DisplayAlert("Éxito", "Actividad eliminada correctamente.", "OK");
                            }
                            else
                            {
                                await DisplayAlert("Error", "No se encontró la actividad.", "OK");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", $"Ocurrió un error al eliminar la actividad: {ex.Message}", "OK");
                    }

                    // Recargar actividades después de la eliminación
                    CargarActividad();
                }
            });
        }

    }
}