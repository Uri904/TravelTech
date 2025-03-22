using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SQLite;
using TravelTech.Tablas;
using Xamarin.Forms;

namespace TravelTech.Views
{
    public partial class MenuLateral : ContentPage
    {
        // Ruta a la base de datos SQLite
        private string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TravelTech.db3");
        private bool viajesExpanded = false;

        public MenuLateral()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Cargar viajes cuando la página aparezca
            CargarViajes();
        }

        private void CargarViajes()
        {
            try
            {
                // Limpiar los elementos existentes
                listaViajesMenu.Children.Clear();

                using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                {
                    // Obtener todos los viajes
                    var viajes = conn.Table<T_Viaje>().ToList();

                    foreach (var viaje in viajes)
                    {
                        // Crear un Frame para cada viaje
                        var frame = new Frame
                        {
                            BackgroundColor = Color.FromHex("#D8E6F7"),
                            CornerRadius = 5,
                            Padding = new Thickness(10),
                            Margin = new Thickness(0, 5)
                        };

                        var grid = new Grid();
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                        // Label para el nombre del viaje
                        var labelViaje = new Label
                        {
                            Text = viaje.nombre,
                            FontSize = 16,
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.Start
                        };
                        grid.Children.Add(labelViaje, 0, 0);

                        // Botón para ver detalles
                        var btnVerDetalles = new Button
                        {
                            Text = "Detalles",
                            BackgroundColor = Color.FromHex("#2A4B7C"),
                            TextColor = Color.White,
                            CornerRadius = 5,
                            HeightRequest = 35,
                            WidthRequest = 90,
                            FontSize = 12,
                            CommandParameter = viaje.Id
                        };
                        btnVerDetalles.Clicked += btn_VerDetallesDesdeMenu;
                        grid.Children.Add(btnVerDetalles, 1, 0);

                        frame.Content = grid;
                        listaViajesMenu.Children.Add(frame);
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Error al cargar viajes: {ex.Message}", "OK");
            }
        }

        private void ToggleViajesLista(object sender, EventArgs e)
        {
            // Cambiar visibilidad de la lista
            viajesExpanded = !viajesExpanded;
            listaViajesMenu.IsVisible = viajesExpanded;

            // Cambiar la imagen de la flecha (asumiendo que tienes dos imágenes: flecha_abajo.png y flecha_arriba.png)
            imgViajesFlecha.Source = viajesExpanded ? "flecha_arriba.png" : "flecha_abajo.png";

            // Si se está expandiendo, asegurarse de que la lista esté actualizada
            if (viajesExpanded)
            {
                CargarViajes();
            }
        }

        private void btn_VerDetallesDesdeMenu(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var viajeId = (int)button.CommandParameter;

            // Cerrar el menú
            (Application.Current.MainPage as FlyoutPage).IsPresented = false;

            // Navegar a la página de detalles
            (Application.Current.MainPage as FlyoutPage).Detail.Navigation.PushAsync(new Viajes.Detalles(viajeId));
        }

        private void IrAInicio(object sender, EventArgs e)
        {
            // Cerrar el menú lateral
            (Application.Current.MainPage as FlyoutPage).IsPresented = false;

            // Limpiar la pila de navegación y volver a la página principal
            (Application.Current.MainPage as FlyoutPage).Detail.Navigation.PopToRootAsync();
        }

        private void IrACalendario(object sender, EventArgs e)
        {
            // Cerrar el menú lateral
            (Application.Current.MainPage as FlyoutPage).IsPresented = false;

            // Navegar a la página de calendario
            (Application.Current.MainPage as FlyoutPage).Detail.Navigation.PushAsync(new CalendarioIn());
        }
    }
}