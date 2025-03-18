using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelTech.Tablas;
using SQLite;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TravelTech.Tablas;
using SQLite;
using System.IO;
using static Xamarin.Essentials.Permissions;

namespace TravelTech.Views.Destinos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class Crear : ContentPage
    {
        private int _viajeId;

        private string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TravelTech.db3");

        public Crear(int viajeId)
        {
            InitializeComponent();
            _viajeId = viajeId;

        }


        // -- Navegación -- //

        //Evento del boton btn_Regresar
        private async void btn_Regresar(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();// Regresa a la página anterior
        }

        private async void btn_AgregarDestino(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(EntryDestino.Text))
            {
                await DisplayAlert("Error", "Por favor ingrese un nombre para el Destino", "OK");
                return;
            }

            if (string.IsNullOrEmpty(EntryNota.Text))
            {
                await DisplayAlert("Error", "Por favor ingrese una nota", "OK");
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


            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                {
                    // Crear un nuevo objeto de destino
                    T_Destino nuevoViaje = new T_Destino
                    {
                        nombre = EntryDestino.Text,
                        PK_id_viaje = _viajeId,
                        prioridad = prioridadSeleccionada,
                        nota = EntryNota.Text
                    };

                    // Insertar en la base de datos
                    int result = conn.Insert(nuevoViaje);

                    if (result > 0)
                    {
                        // Mostrar mensaje de éxito
                        await DisplayAlert("Éxito", "Destino creado correctamente", "OK");

                        // Redirigir a la MainPage
                        await Navigation.PushAsync(new Views.Destinos.VerDestinos(_viajeId));
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo crear el Destino", "OK");
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