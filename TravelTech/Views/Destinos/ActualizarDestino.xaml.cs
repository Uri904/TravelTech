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

namespace TravelTech.Views.Destinos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActualizarDestino : ContentPage
    {
        private int _destinoId;
        private string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TravelTech.db3");

        public ActualizarDestino( int destinoId)
        {
            InitializeComponent();
            _destinoId = destinoId;


        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            CargarDestino();
        }

        // -- Navegación -- //

        //Evento del boton BtnCancelar
        private async void BtnCancelar(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }

        //Evento del boton btn_Regresar
        private async void btn_Regresar(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();// Regresa a la página anterior
        }
        private void CargarDestino()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                {
                    var destino = conn.Table<T_Destino>().FirstOrDefault(d => d.Id == _destinoId); // Buscar por destinoId
                    if (destino != null)
                    {
                        // Mostrar los datos del destino en los controles
                        nombreEntry.Text = destino.nombre;
                        notaEntry.Text = destino.nota;

                        // Seleccionar la prioridad correcta
                        switch (destino.prioridad)
                        {
                            case "Alta":
                                rbAlta.IsChecked = true;
                                break;
                            case "Media":
                                rbMedia.IsChecked = true;
                                break;
                            case "Baja":
                                rbBaja.IsChecked = true;
                                break;
                        }
                    }
                    else
                    {
                        DisplayAlert("Error", "No se encontró el destino", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Error al cargar el destino: {ex.Message}", "OK");
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

                if (string.IsNullOrWhiteSpace(notaEntry.Text))
                {
                    await DisplayAlert("Error", "La nota del viaje es obligatorio", "OK");
                    return;
                }
                string prioridadSeleccionada = null;

                if (rbAlta.IsChecked)
                    prioridadSeleccionada = "Alta";
                else if (rbMedia.IsChecked)
                    prioridadSeleccionada = "Media";
                else if (rbBaja.IsChecked)
                    prioridadSeleccionada = "Baja";

                if (prioridadSeleccionada == null)
                {
                    await DisplayAlert("Error", "Por favor seleccione una prioridad", "OK");
                    return;
                }



                using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                {
                    var destino = conn.Table<T_Destino>().FirstOrDefault(v => v.Id == _destinoId);
                    if (destino != null)
                    {
                        // Actualizar los datos del destino
                        destino.nombre = nombreEntry.Text;
                        destino.prioridad = prioridadSeleccionada;
                        destino.nota = notaEntry.Text;

                        // Guardar los cambios en la base de datos
                        conn.Update(destino);

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
    }
}