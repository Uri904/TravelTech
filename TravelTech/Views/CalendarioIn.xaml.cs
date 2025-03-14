using System;
using System.Globalization;
using Xamarin.Forms;

namespace TravelTech.Views
{
    public partial class CalendarioIn : ContentPage
    {
        private DateTime _currentDate;
        private CultureInfo _culture;

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

                // Resaltar fechas específicas de los viajes
                if (day == 1 || day == 19)
                {
                    var frame = new Frame
                    {
                        BackgroundColor = day == 1 ? Color.FromHex("#4CAF50") : Color.FromHex("#FF5722"),
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
    }
}