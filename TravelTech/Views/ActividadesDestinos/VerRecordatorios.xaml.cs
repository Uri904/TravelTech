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
using SQLiteNetExtensions.Extensions;
using static TravelTech.Views.ActividadesDestinos.VerActividad;
//using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace TravelTech.Views.ActividadesDestinos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerRecordatorios : ContentPage
    {
        private int _viajeId;
        private int recordatorioId;
        private string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TravelTech.db3");

        public VerRecordatorios(int viajeId)
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


        public class RecordatorioDisplayModel
        {
            public int Id { get; set; }
            public string Fecha_recordatorio { get; set; }
            public string Mensaje { get; set; }
            public string estado { get; set; }
            public string importancia { get; set; }
        }



        // -- Navegación -- // 

        //Evento del boton btn_Regresar
        private async void btn_Regresar(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();// Regresa a la página anterior
        }

        //Evento del boton BtnEditar
        private async void BtnEditar(object sender, System.EventArgs e)

        {
            var button = (Button)sender;
            var recordatorioId = (int)button.CommandParameter;
            await Navigation.PushAsync(new Views.ActividadesDestinos.ActualizarRecordatoios(recordatorioId));
        }

        

        //Evento del boton btn_AgregarRecordatorio
        private async void btn_AgregarRecordatorio(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Views.ActividadesDestinos.CrearRecordatorio(_viajeId));
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            CargarRecordatorios();
        }


        private void ToggleRecordatorio(object sender, EventArgs e)
        {
            var button = (Button)sender;

            // Acceder al ViewCell que contiene este botón
            var viewCell = (ViewCell)button.Parent.Parent.Parent;

            if (viewCell != null)
            {
                var contentRecordatorio = viewCell.FindByName<StackLayout>("contentRecordatorio");

                if (contentRecordatorio != null)
                {
                    contentRecordatorio.IsVisible = !contentRecordatorio.IsVisible;
                    button.Text = contentRecordatorio.IsVisible ? "▲" : "▼";
                }
                else
                {
                    DisplayAlert("Error", "No se encontraron los datos del recordatorio.", "OK");
                }
            }
        }

        //See Recordatorios
        private void CargarRecordatorios()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                {
                    // Filtramos los recordatorios por el _viajeId
                    var recordatorios = conn.Table<T_Recordatorio>()
                                            .Where(r => r.FK_id_viaje == _viajeId) // Filtramos por viaje
                                            .ToList();


                    // Verificamos si hay recordatorios
                    if (recordatorios.Count == 0)
                    {
                        labelNoRecordatorios.IsVisible = true; // Muestra el mensaje de que no hay recordatorios
                        listaRecordatorios.IsVisible = false; // Oculta la lista de recordatorios
                    }
                    else
                    {
                        labelNoRecordatorios.IsVisible = false; // Oculta el mensaje
                        listaRecordatorios.IsVisible = true; // Muestra la lista de recordatorios

                        var recordatorioDisplay = new List<RecordatorioDisplayModel>();

                        foreach (var recordatorio in recordatorios)
                        {
                            recordatorioDisplay.Add(new RecordatorioDisplayModel
                            {
                                Id = recordatorio.Id,
                                Fecha_recordatorio = recordatorio.Fecha_recordatorio,
                                Mensaje = recordatorio.Mensaje,
                                estado = recordatorio.estado,
                                importancia = recordatorio.importancia
                            });
                        }

                        // Asignamos los recordatorios a la lista
                        listaRecordatorios.ItemsSource = recordatorioDisplay;
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Error al cargar recordatorios: {ex.Message}", "OK");
            }
        }


        private void Btn_Eliminar(object sender, EventArgs e)
        {
            var button = (ImageButton)sender; // CORREGIDO: Cambiar a ImageButton

            // Validar que el CommandParameter sea correcto
            if (button.CommandParameter == null || !(button.CommandParameter is int recordatorioId))
            {
                DisplayAlert("Error", "No se pudo obtener el ID del recordatorio.", "OK");
                return;
            }

            // Confirmar y eliminar
            Device.BeginInvokeOnMainThread(async () =>
            {
                bool confirmar = await DisplayAlert("Confirmar", "¿Está seguro de eliminar el recordatorio?", "Sí", "No");
                if (confirmar)
                {
                    try
                    {
                        using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                        {
                            conn.Execute("PRAGMA foreign_keys = ON;"); // Asegurar que las llaves foráneas estén activas

                            // Verificar si el recordatorio existe
                            var recordatorio = conn.Find<T_Recordatorio>(recordatorioId);
                            if (recordatorio != null)
                            {
                                conn.Delete(recordatorio);
                                await DisplayAlert("Éxito", "Se eliminó el recordatorio.", "OK");
                            }
                            else
                            {
                                await DisplayAlert("Error", "No se encontró el recordatorio.", "OK");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", $"ERROR al intentar eliminar el recordatorio: {ex.Message}", "OK");
                    }

                    // Recargar recordatorios después de la eliminación
                    CargarRecordatorios();
                }
            });
        }




    }//ContentPage
}//namespace