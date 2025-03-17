using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TravelTech.Tablas;
using SQLite;
using System.IO;

namespace TravelTech.Views.Viajes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Crear : ContentPage
    {
        // Definir la conexión de la base de datos SQLite
        private string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TravelTech.db3");

        public Crear()
        {
            InitializeComponent();
        }

        // Evento del boton btn_Regresar
        private async void btn_Regresar(object sender, EventArgs e)
        {
            await Navigation.PopAsync(); // Regresa a la página anterior
        }

        // Evento del boton btn_CrearViaje
        private async void btn_CrearViaje(object sender, EventArgs e)
        {
            // Validar que los campos no estén vacíos
            if (string.IsNullOrEmpty(EntryNombre.Text))
            {
                await DisplayAlert("Error", "Por favor ingrese un nombre para el viaje", "OK");
                return;
            }

            if (string.IsNullOrEmpty(EntryPresupuesto.Text))
            {
                await DisplayAlert("Error", "Por favor ingrese un presupuesto", "OK");
                return;
            }

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                {
                    // Crear un nuevo objeto de viaje
                    T_Viaje nuevoViaje = new T_Viaje
                    {
                        nombre = EntryNombre.Text,
                        Fecha_inicio = DateInicio.Date.ToString("yyyy-MM-dd"),
                        Fecha_fin = DateFin.Date.ToString("yyyy-MM-dd"),
                        Presupuesto = EntryPresupuesto.Text
                    };

                    // Insertar en la base de datos
                    int result = conn.Insert(nuevoViaje);

                    if (result > 0)
                    {
                        // Mostrar mensaje de éxito
                        await DisplayAlert("Éxito", "Viaje creado correctamente", "OK");

                        // Redirigir a la MainPage
                        await Navigation.PopToRootAsync();
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo crear el viaje", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ocurrió un error: " + ex.Message, "OK");
            }
        }
    }
}