using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;
using SQLite;
using System.IO;
using TravelTech.Tablas;

namespace TravelTech.Views
{
    public partial class CalendarioIn : ContentPage
    {
        private DateTime _currentDate;
        private CultureInfo _culture;
        private string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TravelTech.db3");

        public CalendarioIn()
        {
            InitializeComponent();
            _culture = new CultureInfo("es-ES");
            _currentDate = DateTime.Now;
            UpdateCalendar();
        }

        private void UpdateCalendar()
        {
            // Actualizar título del mes
            MonthYearLabel.Text = _currentDate.ToString("MMMM yyyy", _culture).ToUpper();

            // Limpiar grid anterior
            CalendarGrid.Children.Clear();

            // Calcular primer día del mes y días del mes
            DateTime firstDayOfMonth = new DateTime(_currentDate.Year, _currentDate.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(_currentDate.Year, _currentDate.Month);
            int startingDay = (int)firstDayOfMonth.DayOfWeek;

            // Ajustar inicio de la semana (domingo = 0)
            int col = startingDay;
            int row = 0;

            // Obtener los viajes desde la base de datos
            List<T_Viaje> viajes = GetViajes();

            // Rellenar días vacíos antes del primer día del mes
            for (int day = 1; day <= daysInMonth; day++)
            {
                var dayLabel = new Label
                {
                    Text = day.ToString(),
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Margin = 5
                };

                // Comprobar si la fecha de inicio o fin coincide con algún viaje
                DateTime dayDate = new DateTime(_currentDate.Year, _currentDate.Month, day);

                // Comprobar fechas de inicio de viaje
                var viajeInicio = viajes.Find(v => DateTime.TryParse(v.Fecha_inicio, out DateTime startDate) && startDate.Date == dayDate.Date);
                if (viajeInicio != null)
                {
                    // Resaltar fecha de inicio de viaje en verde
                    var frame = new Frame
                    {
                        BackgroundColor = Color.FromHex("#4CAF50"), // Verde para inicio
                        CornerRadius = 15,
                        HeightRequest = 30,
                        WidthRequest = 30,
                        Padding = 0,
                        Content = new Label
                        {
                            Text = day.ToString(),
                            TextColor = Color.White,
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center
                        }
                    };
                    Grid.SetColumn(frame, col);
                    Grid.SetRow(frame, row);
                    CalendarGrid.Children.Add(frame);
                }

                // Comprobar fechas de fin de viaje
                var viajeFin = viajes.Find(v => DateTime.TryParse(v.Fecha_fin, out DateTime endDate) && endDate.Date == dayDate.Date);
                if (viajeFin != null)
                {
                    // Resaltar fecha de fin de viaje en rojo
                    var frame = new Frame
                    {
                        BackgroundColor = Color.FromHex("#FF5722"), // Rojo para fin
                        CornerRadius = 15,
                        HeightRequest = 30,
                        WidthRequest = 30,
                        Padding = 0,
                        Content = new Label
                        {
                            Text = day.ToString(),
                            TextColor = Color.White,
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center
                        }
                    };
                    Grid.SetColumn(frame, col);
                    Grid.SetRow(frame, row);
                    CalendarGrid.Children.Add(frame);
                }
                else
                {
                    // Si no es un día de viaje, agregar solo el día
                    Grid.SetColumn(dayLabel, col);
                    Grid.SetRow(dayLabel, row);
                    CalendarGrid.Children.Add(dayLabel);
                }

                col++;
                if (col > 6)
                {
                    col = 0;
                    row++;
                }
            }
        }

        private List<T_Viaje> GetViajes()
        {
            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                // Obtener todos los viajes desde la base de datos
                return conn.Table<T_Viaje>().ToList();
            }
        }

        private void OnPreviousMonthClicked(object sender, EventArgs e)
        {
            _currentDate = _currentDate.AddMonths(-1);
            UpdateCalendar();
        }

        private void OnNextMonthClicked(object sender, EventArgs e)
        {
            _currentDate = _currentDate.AddMonths(1);
            UpdateCalendar();
        }



        // -- Navegación -- //

        //Evento btn_Home
        private async void btn_Home(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        //Evento btn_MisViajes
        private async void btn_MisViajes(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        //Evento btn_Crear
        private async void btn_CrearViaje(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Viajes.Crear());
        }

        private void btn_AbrirMenu(object sender, EventArgs e)
        {
            // Mostrar el menú lateral
            (Application.Current.MainPage as FlyoutPage).IsPresented = true;
        }

    }
}