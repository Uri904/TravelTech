using System;
using System.IO;
using SQLite;
using TravelTech.Tablas;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelTech.Views.Gastos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AgregarGasto : ContentPage
    {
        private string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TravelTech.db3");
        private int _viajeId;  // ID del viaje al que pertenece el gasto

        public AgregarGasto(int viajeId)  // Recibe el ID del viaje
        {
            InitializeComponent();
            _viajeId = viajeId; // Asignar el ID del viaje al que pertenece el gasto
        }

        private async void BtnAgregarGasto_Clicked(object sender, EventArgs e)
        {
            // Validar que los campos no estén vacíos
            if (string.IsNullOrEmpty(txtCategoria.Text) || string.IsNullOrEmpty(txtMonto.Text))
            {
                await DisplayAlert("Error", "Todos los campos son obligatorios.", "OK");
                return;
            }

            // Validar que el monto sea un número válido
            if (!double.TryParse(txtMonto.Text, out double monto))
            {
                await DisplayAlert("Error", "El monto debe ser un número válido.", "OK");
                return;
            }

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                {
                    conn.CreateTable<T_Gasto>();  // Asegurarse de que la tabla existe

                    // Crear el objeto Gasto
                    T_Gasto nuevoGasto = new T_Gasto
                    {
                        PK_id_viaje = _viajeId,  // Relación con el viaje
                        Categoria = txtCategoria.Text,
                        Monto = monto,
                        Fecha_gasto = dpFecha.Date
                    };

                    // Insertar en la base de datos
                    int result = conn.Insert(nuevoGasto);
                    System.Diagnostics.Debug.WriteLine($"Gasto insertado con resultado: {result}");  // Depuración

                    if (result > 0)
                    {
                        await DisplayAlert("Éxito", "Gasto agregado correctamente.", "OK");

                        // Regresar a la pantalla anterior y actualizar los datos de la lista
                        await Navigation.PopAsync();

                        // Actualizar la lista de gastos en la página anterior
                        if (this.Navigation.NavigationStack.Count > 1) // Evita error de índice
                        {
                            var parentPage = this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 2] as VerGastos;
                            parentPage?.CargarGastos();  // Refrescar solo si es válido
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo agregar el gasto.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ocurrió un error: " + ex.Message, "OK");
            }
        }

        // -- Navegación -- //

        //Evento btn_Regresar
        private async void btn_Regresar(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();// Regresa a la página anterior
        }

        //Evento del boton BtnCancelar
        private async void BtnCancelar(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();// Regresa a la página anterior
        }
    }
}
