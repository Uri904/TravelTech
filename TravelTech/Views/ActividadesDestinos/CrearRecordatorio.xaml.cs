using SQLite;
using System;
using System.Collections.Generic;
using TravelTech.Tablas;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.IO;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace TravelTech.Views.ActividadesDestinos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrearRecordatorio : ContentPage
    {

        private int _viajeId;
        private SQLiteConnection database;
        private List<T_Actividad> ListaActividades = new List<T_Actividad>(); // Lista de actividades
        private string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TravelTech.db3");

        public CrearRecordatorio(int viajeId)
        {
            InitializeComponent();
            _viajeId = viajeId;
            database = new SQLiteConnection(dbPath);


            using (var db = new SQLiteConnection(dbPath))
            {
                db.CreateTable<T_Viaje>();
                db.CreateTable<T_Recordatorio>();
                db.CreateTable<T_Destino>();
                db.CreateTable<T_Actividad>();
                db.CreateTable<T_Gasto>();
            }



        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            CargarActividad();
        }

        // -- Navegación -- // 

        //Evento del boton btn_Regresar
        private async void btn_Regresar(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();// Regresa a la página anterior
        }


        private void CargarActividad()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                {
                    var actividades = conn.Table<T_Actividad>()
                                          .Where(a => a.PK_id_viaje == _viajeId) // Filtramos por viajeId
                                          .ToList();

                    // Asignamos las actividades a la lista para usarlas más tarde
                    ListaActividades = actividades;

                    // Asignamos los nombres de las actividades al Picker
                    pkActividad.ItemsSource = actividades.Select(a => a.nombre).ToList();
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Error al cargar actividades: {ex.Message}", "OK");
            }
        }


        private async void btn_CrearRecordatorio(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(Mensaje.Text))
            {
                await DisplayAlert("Error", "Ingresa un mensaje", "OK");
                return;
            }
            if (!iAlta.IsChecked && !iMedia.IsChecked && !iBaja.IsChecked)
            {
                await DisplayAlert("Error", "Selecciona una opción", "OK");
                return;
            }

            if (pkActividad.SelectedIndex == -1)
            {
                await DisplayAlert("Error", "Por favor, selecciona una actividad.", "OK");
                return;
            }

            if (pkEstado.SelectedIndex == -1)
            {
                await DisplayAlert("Error", "Por favor, selecciona un estado.", "OK");
                return;
            }

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                {
                    string importancia;

                    if (iAlta.IsChecked)
                    {
                        importancia = "Alta";
                    }
                    else if (iMedia.IsChecked)
                    {
                        importancia = "Media";
                    }
                    else if (iBaja.IsChecked)
                    {
                        importancia = "Baja";
                    }
                    else
                    {
                        importancia = "No especificado"; // En caso de que ninguno esté seleccionado
                    }

                    // Se obtiene el ID de la actividad seleccionada
                    var actividadId = ListaActividades[pkActividad.SelectedIndex].Id;

                    T_Recordatorio objActividad = new T_Recordatorio
                    {
                        Fecha_recordatorio = FechaRecordatorio.Date.ToString("yyyy-MM-dd"),
                        Mensaje = Mensaje.Text,
                        estado = pkEstado.SelectedItem.ToString(), 
                        importancia = importancia,
                        // Asignamos el viajeId y el actividadId
                        FK_id_actividad = actividadId,  // Aquí asociamos la actividad seleccionada
                        FK_id_viaje = _viajeId  // Asegúrate de que también guardes el viajeId
                    };

                    int resultado = conn.Insert(objActividad);
                    if (resultado > 0)
                    {
                        await DisplayAlert("Éxito", "Recordatorio creado correctamente", "OK");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo registrar el recordatorio", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ocurrió un error: " + ex.Message, "OK");
            }
        }





        //Evento del boton btn_Cancelar
        private async void btn_Cancelar(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();// Regresa a la página anterior
        }
    }
}




