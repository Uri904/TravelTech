using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using TravelTech.Tablas;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelTech.Views.Viajes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditarDetalles : ContentPage
    {
        private int _viajeId;
        private string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TravelTech.db3");

        public EditarDetalles(int viajeId)
        {
            InitializeComponent();
            _viajeId = viajeId;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CargarDatosViaje();
        }

        private void CargarDatosViaje()
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

                        presupuestoEntry.Text = viaje.Presupuesto;
                    }
                    else
                    {
                        DisplayAlert("Error", "No se encontró el viaje seleccionado", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Error al cargar datos del viaje: {ex.Message}", "OK");
            }
        }

        private async void btn_Actualizar(object sender, EventArgs e)
        {
            try
            {
                // Validar datos
                if (string.IsNullOrWhiteSpace(nombreEntry.Text))
                {
                    await DisplayAlert("Error", "El nombre del viaje es obligatorio", "OK");
                    return;
                }

                // El presupuesto, se toma como STRING y no como int o decimal
                string presupuesto = presupuestoEntry.Text;

                using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                {
                    var viaje = conn.Table<T_Viaje>().FirstOrDefault(v => v.Id == _viajeId);
                    if (viaje != null)
                    {
                        // Actualizar los datos del viaje
                        viaje.nombre = nombreEntry.Text;
                        viaje.Fecha_inicio = fechaInicioDatePicker.Date.ToString("yyyy-MM-dd");
                        viaje.Fecha_fin = fechaFinDatePicker.Date.ToString("yyyy-MM-dd");
                        viaje.Presupuesto = presupuesto; // Asignar directamente como string

                        // Guardar los cambios en la base de datos
                        conn.Update(viaje);

                        await DisplayAlert("Éxito", "Los cambios se han guardado correctamente", "OK");
                        await Navigation.PopAsync(); // Volver a la pantalla anterior
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se encontró el viaje para actualizar", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al actualizar el viaje: {ex.Message}", "OK");
            }
        }

        // -- Navegación -- //
        //Evento del btn_Regresar
        private async void btn_Regresar(object sender, EventArgs e)
        {
            await Navigation.PopAsync(); // Regresa a la página anterior
        }

        //Evento del btn_Cancelar
        private async void btn_Cancelar(object sender, EventArgs e)
        {
            await Navigation.PopAsync(); // Regresa a la página anterior
        }
    }
}