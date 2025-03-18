using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using TravelTech.Tablas;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;

namespace TravelTech.Views.Gastos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerGastos : ContentPage
    {
        private int _viajeId;
        private string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TravelTech.db3");

        public VerGastos(int viajeId)
        {
            InitializeComponent();
            _viajeId = viajeId;
            BindingContext = this;  // Establecemos el BindingContext a la página actual
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CargarGastos();
        }

        public void CargarGastos()
        {
            using (var conn = new SQLiteConnection(dbPath))
            {
                var viaje = conn.Table<T_Viaje>().FirstOrDefault(v => v.Id == _viajeId);

                if (viaje != null)
                {
                    // Asegurarse de que el presupuesto es un número
                    double presupuesto = 0;
                    if (double.TryParse(viaje.Presupuesto, out presupuesto))
                    {
                        lblPresupuesto.Text = $"${presupuesto}";
                    }
                    else
                    {
                        lblPresupuesto.Text = "$0";
                    }

                    // Obtener la lista de gastos
                    var gastos = conn.Table<T_Gasto>().Where(g => g.PK_id_viaje == _viajeId).ToList();
                    listaGastos.ItemsSource = gastos;

                    // Calcular totales de los gastos
                    double totalGastos = gastos.Sum(g => g.Monto);
                    lblTotalGastos.Text = $"${totalGastos:0.##}";

                    // Calcular presupuesto restante
                    double presupuestoRestante = presupuesto - totalGastos;
                    lblPresupuestoRestante.Text = $"${presupuestoRestante:0.##}";
                }
            }
        }

        // Evento del Boton BtnAgregar
        private async void BtnAgregar(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AgregarGasto(_viajeId));
        }

        // Evento del Botón Actualizar
        public Command<int> ActualizarCommand => new Command<int>((gastoId) =>
        {
            // Navegar a la página de ActualizarGasto pasando el Id del gasto
            Navigation.PushAsync(new ActualizarGasto(gastoId));
        });

        // Evento del Botón Eliminar
        private async void EliminarButton_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var gastoId = (int)button.CommandParameter;

            bool confirm = await DisplayAlert("Confirmación", "¿Estás seguro de que quieres eliminar este gasto?", "Sí", "No");
            if (confirm)
            {
                using (var conn = new SQLiteConnection(dbPath))
                {
                    var gasto = conn.Table<T_Gasto>().FirstOrDefault(g => g.Id == gastoId);
                    if (gasto != null)
                    {
                        conn.Delete(gasto); // Eliminar el gasto de la base de datos
                        await DisplayAlert("Éxito", "Gasto eliminado correctamente.", "OK");
                        CargarGastos();  // Recargar la lista de gastos después de la eliminación
                    }
                }
            }
        }

        // Eventos de Navegación
        private async void btn_Home(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        private async void btn_Detalles(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Viajes.Detalles(_viajeId));
        }

        private async void btn_Destinos(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Destinos.VerDestinos(_viajeId));
        }

        private async void btn_Actividades(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ActividadesDestinos.VerActividad());
        }
    }
}
