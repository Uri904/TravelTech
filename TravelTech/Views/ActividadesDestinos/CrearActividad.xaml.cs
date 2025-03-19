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

namespace TravelTech.Views.ActividadesDestinos
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CrearActividad : ContentPage
	{
        private string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TravelTech.db3");
        public CrearActividad ()
		{
			InitializeComponent ();
		}


        // -- Navegación -- //

        //Evento del boton btn_Regresar
        private async void btn_Regresar(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();// Regresa a la página anterior
        }

        // Para crear una nueva actividad
        private async void btn_CrearActividad(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(NombreActividad.Text))
            {
                await DisplayAlert("Error", "Ingresa el nombre de la actividad", "OK");
                return;
            }
            if (!rbCompletada.IsChecked && !rbEnProceso.IsChecked && !rbPendiente.IsChecked)
            {
                await DisplayAlert("Error", "Selecciona una opción", "OK");
                return;
            }

            if (string.IsNullOrEmpty(NotasActividad.Text))
            {
                await DisplayAlert("Error", "Ingresa al menos una nota", "OK");
                return;
            }// Para no dejar campos nulos

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                {
                    string estado;

                    if (rbCompletada.IsChecked)
                    {
                        estado = "Completada";
                    }
                    else if (rbEnProceso.IsChecked)
                    {
                        estado = "En Proceso";
                    }
                    else if (rbPendiente.IsChecked)
                    {
                        estado = "Cancelada";
                    }
                    else
                    {
                        estado = "No especificado"; // En caso de que ninguno esté seleccionado
                    }

                    T_Actividad objActividad = new T_Actividad
                    {
                        nombre = NombreActividad.Text,
                        Fecha_actividad = FechaActividad.Date.ToString("yyyy-MM-dd"),
                        Estado = estado,
                        nota = NotasActividad.Text
                    };


                    int resultado = conn.Insert(objActividad); //insersión en la base de datos

                    if (resultado > 0)
                    {

                        await DisplayAlert("Éxito", "Actividad creada correctamente", "OK");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo registrar la actividad", "OK");
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