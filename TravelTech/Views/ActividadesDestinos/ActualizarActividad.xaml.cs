using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using TravelTech.Tablas;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Xamarin.Essentials.Permissions;

namespace TravelTech.Views.ActividadesDestinos
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ActualizarActividad : ContentPage
	{
        private int _actividadId;
        private string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TravelTech.db3");
        public ActualizarActividad (int actividadId)
		{
			InitializeComponent ();
            _actividadId = actividadId;

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            CargarDatosActividad();
        }
        private void CargarDatosActividad()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                {
                    var actividad = conn.Table<T_Actividad>().FirstOrDefault(a => a.Id == _actividadId);
                    if (actividad != null)
                    {
                        nombreActividadA.Text = actividad.nombre;

                        if (DateTime.TryParse(actividad.Fecha_actividad, out DateTime fechaAc))
                        {
                            fechaActividadA.Date = fechaAc;
                        }
                        switch (actividad.Estado)
                        {
                            case "Completada":
                                rbCompletadaA.IsChecked = true;
                                break;
                            case "En Proceso":
                                rbEnProcesoA.IsChecked = true;
                                break;
                            case "Pendiente":
                                rbPendienteA.IsChecked = true;
                                break;
                        }

                        notasActividadA.Text = actividad.nota;
                    }
                    else
                    {
                        DisplayAlert("Error", "No se encontró la actividad seleccionada", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Error al cargar datos de la actividad: {ex.Message}", "OK");
            }
        }

        private async void btn_Actualizar(object sender, EventArgs e)
        {
            try
            {
                // Validar datos
                if (string.IsNullOrWhiteSpace(nombreActividadA.Text))
                {
                    await DisplayAlert("Error", "Ingresa un nombre", "OK");
                    return;
                }
                if (string.IsNullOrWhiteSpace(notasActividadA.Text))
                {
                    await DisplayAlert("Error", "Ingresa al menos una nota", "OK");
                    return;
                }



                using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                {
                    var actividad = conn.Table<T_Actividad>().FirstOrDefault(a => a.Id == _actividadId);
                    if (actividad != null)
                    {
                        string estado;

                        if (rbCompletadaA.IsChecked)
                        {
                            estado = "Completada";
                        }
                        else if (rbEnProcesoA.IsChecked)
                        {
                            estado = "En Proceso";
                        }
                        else if (rbPendienteA.IsChecked)
                        {
                            estado = "Pendiente";
                        }
                        else
                        {
                            estado = "No especificado";
                        }
                        // Actualizar los datos del viaje
                        actividad.nombre = nombreActividadA.Text;
                        actividad.Fecha_actividad = fechaActividadA.Date.ToString("yyyy-MM-dd");
                        actividad.Estado = estado;
                        actividad.nota = notasActividadA.Text;
                        

                        // Guardar los cambios en la base de datos
                        conn.Update(actividad);

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

        //Evento del boton btn_Regresar
        private async void btn_Regresar(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();// Regresa a la página anterior
        }

        //Evento del boton btn_Cancelar
        private async void btn_Cancelar(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();// Regresa a la página anterior
        }

        



    }
}
