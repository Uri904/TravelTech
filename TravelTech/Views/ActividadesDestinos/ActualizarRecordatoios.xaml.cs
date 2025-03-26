using System;
using System.IO;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TravelTech.Tablas;

namespace TravelTech.Views.ActividadesDestinos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActualizarRecordatoios : ContentPage
    {
      
        private int _recordatorioId;
        private string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TravelTech.db3");
        public ActualizarRecordatoios(int recordatorioId)
        {
            InitializeComponent();
            _recordatorioId = recordatorioId;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CargarInfRecordatorios();
        }



        private void CargarInfRecordatorios()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                {
                    var recordatorio = conn.Table<T_Recordatorio>().FirstOrDefault(a => a.Id == _recordatorioId);
                    if (recordatorio != null)
                    {

                        txMensaje.Text = recordatorio.Mensaje;
                        switch (recordatorio.estado)
                        {
                            case "Alta":
                                RbAlta.IsChecked = true;
                                break;
                            case "Media":
                                RbMedia.IsChecked = true;
                                break;
                            case "Baja":
                                RbBaja.IsChecked = true;
                                break;
                        }

                        if (DateTime.TryParse(recordatorio.Fecha_recordatorio, out DateTime fecha_R))
                        {
                            DpFecha.Date = fecha_R;
                        }

                        // Cargar el estado si es válido
                        if (!string.IsNullOrEmpty(recordatorio.estado))
                        {
                            pkEstado.SelectedItem = recordatorio.estado;
                        }

                    }
                    else
                    {
                        DisplayAlert("Error", "No se encontraron recordatorios", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Error al cargar la información del recordatorio: {ex.Message}", "OK");
            }
        }

        private async void Btn_Actualizar(object sender, EventArgs e)
        {
            try
            {
                // Validar datos
                if (string.IsNullOrWhiteSpace(txMensaje.Text))
                {
                    await DisplayAlert("Error", "Ingresa un nombre", "OK");
                    return;
                }

                using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                {
                    var recordatorio = conn.Table<T_Recordatorio>().FirstOrDefault(a => a.Id == _recordatorioId);
                    if (recordatorio != null)
                    {
                        string importancia;

                        if (RbAlta.IsChecked)
                        {
                            importancia = "Alta";
                        }
                        else if (RbMedia.IsChecked)
                        {
                            importancia = "Media";
                        }
                        else if (RbBaja.IsChecked)
                        {
                            importancia = "Baja";
                        }
                        else
                        {
                            importancia = "No especificado";
                        }
                        // Actualizar los datos del viaje
                        recordatorio.Mensaje = txMensaje.Text;
                        recordatorio.Fecha_recordatorio= DpFecha.Date.ToString("yyyy-MM-dd");
                        recordatorio.estado = pkEstado.SelectedItem.ToString();
                        recordatorio.importancia = importancia;


                        // Guardar los cambios en la base de datos
                        conn.Update(recordatorio);

                        await DisplayAlert("Éxito", "Se actualizaron los datos correctamente", "OK");
                        await Navigation.PopAsync(); // Volver a la pantalla anterior
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se encontró el recordatorio para actualizar", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al actualizar el recordatorio: {ex.Message}", "OK");
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