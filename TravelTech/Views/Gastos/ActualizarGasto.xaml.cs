using System;
using System.IO;
using SQLite;
using TravelTech.Tablas;
using Xamarin.Forms;

namespace TravelTech.Views.Gastos
{
    public partial class ActualizarGasto : ContentPage
    {
        private string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TravelTech.db3");
        private int _gastoId;

        public ActualizarGasto(int gastoId)  // Recibe el ID del gasto a actualizar
        {
            InitializeComponent();
            _gastoId = gastoId; // Guardar el ID para poder hacer la actualización o eliminación
            CargarGasto();  // Cargar el gasto en los controles
        }

        private void CargarGasto()
        {
            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                var gasto = conn.Table<T_Gasto>().FirstOrDefault(g => g.Id == _gastoId);
                if (gasto != null)
                {
                    BindingContext = gasto; // Asignar el objeto Gasto al BindingContext
                }
            }
        }

        // Evento para regresar a la pantalla anterior
        private async void btn_Regresar(object sender, EventArgs e)
        {
            await Navigation.PopAsync(); // Regresa a la página anterior
        }

        // Evento para cancelar la operación
        private async void BtnCancelar(object sender, EventArgs e)
        {
            await Navigation.PopAsync(); // Regresa sin hacer cambios
        }

        // Evento para actualizar el gasto
        private async void BtnActualizar(object sender, EventArgs e)
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
                    var gasto = conn.Table<T_Gasto>().FirstOrDefault(g => g.Id == _gastoId);
                    if (gasto != null)
                    {
                        gasto.Categoria = txtCategoria.Text;
                        gasto.Monto = monto;
                        gasto.Fecha_gasto = dpFecha.Date;

                        int result = conn.Update(gasto);

                        if (result > 0)
                        {
                            await DisplayAlert("Éxito", "Gasto actualizado correctamente.", "OK");
                            await Navigation.PopAsync();  // Regresar a la pantalla anterior
                        }
                        else
                        {
                            await DisplayAlert("Error", "No se pudo actualizar el gasto.", "OK");
                        }
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

