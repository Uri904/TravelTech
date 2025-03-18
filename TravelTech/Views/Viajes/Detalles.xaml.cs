using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using System.IO;
using TravelTech.Tablas;

namespace TravelTech.Views.Viajes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Detalles : ContentPage
    {
        private int _viajeId;
        private string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TravelTech.db3");

        public Detalles(int viajeId)
        {
            InitializeComponent();
            _viajeId = viajeId;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CargarDetallesViaje();
        }

        private void CargarDetallesViaje()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                {
                    var viaje = conn.Table<T_Viaje>().FirstOrDefault(v => v.Id == _viajeId);
                    if (viaje != null)
                    {
                        
                        nombreEntry.Text = viaje.nombre;
                        if (DateTime.TryParse(viaje.Fecha_inicio, out DateTime fechaInicio))
                        {
                            fechaInicioDatePicker.Date = fechaInicio;
                        }
                        if (DateTime.TryParse(viaje.Fecha_fin, out DateTime fechaFin))
                        {
                            fechaFinDatePicker.Date = fechaFin;
                        }
                        presupuestoEntry.Text = viaje.Presupuesto.ToString();
                    }
                    else
                    {
                        DisplayAlert("Error", "No se encontró el viaje seleccionado", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Error al cargar detalles del viaje: {ex.Message}", "OK");
            }
        }

        // -- Navegación -- //
        private async void btn_Home(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        //Evento btn_Destino
        private async void btn_Destino(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Destinos.VerDestinos(_viajeId));
        }

        //Evento btn_Actividades
        private async void btn_Actividades(object sender, System.EventArgs e)
        {
            //await Navigation.PushAsync(new ActividadesDestinos.VerActividad(_viajeId));
        }

        //Evento btn_Gastos
        private async void btn_Gastos(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Gastos.VerGastos(_viajeId));
        }

        //Evento btn_EditarDetalles
        private async void btn_EditarDetalles(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Viajes.EditarDetalles(_viajeId));
        }
    }
}