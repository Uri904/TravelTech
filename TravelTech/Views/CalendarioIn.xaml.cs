using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;
using SQLite;
using System.IO;
using TravelTech.Tablas;
using System.Linq;
using Xamarin.Essentials;
//using UserNotifications;





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
            CargarViajeDelMes(DateTime.Now.Month, DateTime.Now.Year);
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
            CargarViajeDelMes(_currentDate.Month, _currentDate.Year);
        }

        private void OnNextMonthClicked(object sender, EventArgs e)
        {
            _currentDate = _currentDate.AddMonths(1);
            UpdateCalendar();
                CargarViajeDelMes(_currentDate.Month, _currentDate.Year);
        }



        //Método para obtener los viajes del mes
        private void CargarViajeDelMes(int mes, int año)
        {
            // Limpiar la lista antes de agregar nuevos elementos
            ProximosViajesStack.Children.Clear();

            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                conn.CreateTable<T_Viaje>();

                // Obtener todos los viajes de la base de datos
                var viajes = conn.Table<T_Viaje>().ToList();

                // Filtrar los viajes por mes y año
                var viajesDelMes = viajes.Where(v =>
                {
                    DateTime fechaInicio, fechaFin;
                    bool fechaInicioValida = DateTime.TryParse(v.Fecha_inicio, out fechaInicio);
                    bool fechaFinValida = DateTime.TryParse(v.Fecha_fin, out fechaFin);

                    return fechaInicioValida && fechaInicio.Month == mes && fechaInicio.Year == año;
                }).ToList();

                if (!viajesDelMes.Any())
                {
                    // Si no hay viajes para este mes, mostrar un mensaje
                    ProximosViajesStack.Children.Add(new Label
                    {
                        Text = "No hay viajes programados para este mes.",
                        TextColor = Color.Gray,
                        FontAttributes = FontAttributes.Italic,
                        HorizontalOptions = LayoutOptions.Center
                    });
                    return;
                }

                foreach (var viajeDelMes in viajesDelMes)
                {
                    DateTime fechaInicio = DateTime.Parse(viajeDelMes.Fecha_inicio);
                    DateTime fechaFin = DateTime.Parse(viajeDelMes.Fecha_fin);

                    var frame = new Frame
                    {
                        Padding = 15,
                        BackgroundColor = Color.White,
                        CornerRadius = 10,
                        HasShadow = true,
                        Content = new Grid
                        {
                            ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    },
                            Children =
                    {
                        new StackLayout
                        {
                            Children =
                            {
                                new Label { Text = viajeDelMes.nombre, FontAttributes = FontAttributes.Bold, FontSize = 18 },
                                new Label { Text = $"Fecha de inicio: {fechaInicio.ToString("d MMMM, yyyy")}", TextColor = Color.Gray },
                                new Label { Text = $"Fecha de fin: {fechaFin.ToString("d MMMM, yyyy")}", TextColor = Color.Gray }
                            }
                        },
                        new Button
                        {
                            Text = "Ver detalles",
                            BackgroundColor = Color.FromHex("#2196F3"),
                            TextColor = Color.White,
                            CornerRadius = 20,
                            HorizontalOptions = LayoutOptions.End,
                            VerticalOptions = LayoutOptions.Center,
                            Command = new Command(() => VerDetallesViaje(viajeDelMes.Id))
                        }
                    }
                        }
                    };

                    // Agregar el Frame al StackLayout
                    ProximosViajesStack.Children.Add(frame);
                }
            }
        }


        // Método para ver detalles del viaje
        private async void VerDetallesViaje(int viajeId)
        {
            // Navegar a la página de detalles del viaje
            await Navigation.PushAsync(new Viajes.Detalles(viajeId));
        }

        // Evento para el calendario
        private void CalendarDateSelected(object sender, DateChangedEventArgs e)
        {
            // Obtener el mes y año de la fecha seleccionada
            int mes = e.NewDate.Month;
            int año = e.NewDate.Year;

            // Cargar el viaje correspondiente al mes y año seleccionados
            CargarViajeDelMes(mes, año);
            var viajes = GetViajes();
            var viajeSeleccionado = viajes.FirstOrDefault(v =>
                DateTime.TryParse(v.Fecha_inicio, out DateTime startDate) &&
                startDate.Date == e.NewDate.Date);

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