using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;
using SQLite;
using System.IO;
using TravelTech.Tablas;
using System.Linq;
using Xamarin.Essentials;
using System.Threading.Tasks;
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

            // Verificar si algún viaje comienza hoy y enviar el mensaje de WhatsApp
            foreach (var viaje in viajes)
            {
                DateTime fechaInicio = DateTime.Parse(viaje.Fecha_inicio);
                EnviarMensajeWhatsApp(fechaInicio, viaje.nombre);
            }

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

                // Buscar el viaje cuyo inicio o fin coincidan con la fecha
                var viajeInicio = viajes.FirstOrDefault(v => DateTime.TryParse(v.Fecha_inicio, out DateTime startDate) && startDate.Date == dayDate.Date);
                var viajeFin = viajes.FirstOrDefault(v => DateTime.TryParse(v.Fecha_fin, out DateTime endDate) && endDate.Date == dayDate.Date);

                if (viajeInicio != null || viajeFin != null)
                {
                    var viaje = viajeInicio ?? viajeFin; // Usa el viaje de inicio si existe, o el de fin si no

                    // Asignar color único para este viaje
                    var viajeColor = GetViajeColor(viaje.Id);

                    var frame = new Frame
                    {
                        BackgroundColor = viajeColor,
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


        // Función para obtener un color único para cada fecha (por ejemplo, basado en el Id del viaje)
        /* private Color GetColorForDate(DateTime date, List<T_Viaje> viajes)
         {
             // Encuentra todos los viajes que coincidan con la fecha
             var viajesParaElDia = viajes.Where(v =>
                 DateTime.TryParse(v.Fecha_inicio, out DateTime startDate) &&
                 startDate.Date == date.Date).ToList();

             // Si no hay viajes para esa fecha, usar un color neutro
             if (!viajesParaElDia.Any())
             {
                 return Color.Gray;
             }

             // Si hay viajes para esa fecha, asignar un color basado en el Id del viaje
             var viaje = viajesParaElDia.First();
             int viajeId = viaje.Id;

             // Generar un color único (puedes ajustar esto a tu gusto)
             Random random = new Random(viajeId);
             return Color.FromRgb(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256));
         }*/



        //Lista de colores para asociar fechas del calendatio con Viajes
        private List<Color> viajeColors = new List<Color>
{
    Color.FromHex("#FF6347"), // Tomato
    Color.FromHex("#4682B4"), // SteelBlue
    Color.FromHex("#32CD32"), // LimeGreen
    Color.FromHex("#FFD700"), // Gold
    Color.FromHex("#8A2BE2"), // BlueViolet
    Color.FromHex("#FF4500"), // OrangeRed
    Color.FromHex("#2E8B57"), // SeaGreen
    Color.FromHex("#6A5ACD"), // SlateBlue
    Color.FromHex("#00CED1"), // DarkTurquoise
    Color.FromHex("#FF1493"), // DeepPink
    Color.FromHex("#F08080"), // LightCoral
    Color.FromHex("#8B4513"), // SaddleBrown
    Color.FromHex("#ADFF2F"), // GreenYellow
    Color.FromHex("#B22222"), // Firebrick
    Color.FromHex("#7FFF00"), // Chartreuse
    Color.FromHex("#DC143C"), // Crimson
    Color.FromHex("#00BFFF"), // DeepSkyBlue
    Color.FromHex("#9400D3"), // DarkViolet
    Color.FromHex("#FF8C00"), // DarkOrange
    Color.FromHex("#4B0082"), // Indigo
    Color.FromHex("#8B0000"), // DarkRed
    Color.FromHex("#DAA520"), // GoldenRod
    Color.FromHex("#E0FFFF"), // LightCyan
    Color.FromHex("#FA8072"), // Salmon
    Color.FromHex("#FF69B4"), // HotPink
    Color.FromHex("#20B2AA"), // LightSeaGreen
    Color.FromHex("#D2691E"), // Chocolate
    Color.FromHex("#A52A2A"), // Brown
    Color.FromHex("#9ACD32"), // YellowGreen
    Color.FromHex("#FFB6C1"), // LightPink
    Color.FromHex("#BDB76B"), // DarkKhaki
    Color.FromHex("#F0E68C"), // Khaki
    Color.FromHex("#3CB371"), // MediumSeaGreen
    Color.FromHex("#FF00FF"), // Magenta
    Color.FromHex("#D3D3D3"), // LightGray
    Color.FromHex("#C71585"), // MediumVioletRed
    Color.FromHex("#F4A300"), // Mustard
    Color.FromHex("#7B68EE"), // MediumSlateBlue
    Color.FromHex("#32CD32"), // LimeGreen
    Color.FromHex("#FF7F50"), // Coral
    Color.FromHex("#800080"), // Purple
    Color.FromHex("#48D1CC"), // MediumTurquoise
    Color.FromHex("#C0C0C0"), // Silver
    Color.FromHex("#B0C4DE"), // LightSteelBlue
};


        private Color GetViajeColor(int viajeId)
        {
            // Obtener color único basado en el Id del viaje
            return viajeColors[viajeId % viajeColors.Count];  // Si el Id supera el número de colores, se repiten los colores
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

                    // Obtener el color único para este viaje
                    var viajeColor = GetViajeColor(viajeDelMes.Id);

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
                            BackgroundColor = viajeColor, // Usar el mismo color que en el calendario
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


        private async void EnviarMensajeWhatsApp(DateTime fechaInicio, string viajeNombre)
        {
            DateTime fechaHoy = DateTime.Now;

            // Verificar si la fecha de inicio es hoy
            if (fechaInicio.Date == fechaHoy.Date)
            {
                var phoneNumber = "5584116500"; // Aquí va el número de teléfono
                var message = $"¡Hola! Hoy comienza tu viaje: {viajeNombre}.";

                try
                {
                    // Asegurarse de que el texto esté correctamente codificado para URL
                    var encodedMessage = Uri.EscapeDataString(message);
                    var encodedPhoneNumber = Uri.EscapeDataString(phoneNumber);

                    // Crear el URI de WhatsApp
                    var uri = new Uri($"whatsapp://send?phone={encodedPhoneNumber}&text={encodedMessage}");

                    // Verificar si WhatsApp está instalado
                    var whatsappInstalled = await CanOpenWhatsApp();

                    if (whatsappInstalled)
                    {
                        // Intentar abrir WhatsApp con el URI
                        await Launcher.OpenAsync(uri);
                        await DisplayAlert("Mensaje enviado", "El mensaje de WhatsApp fue enviado.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Error", "WhatsApp no está instalado en este dispositivo.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"No se pudo enviar el mensaje: {ex.Message}", "OK");
                }
            }
        }

        // Método para verificar si WhatsApp está instalado
        private async Task<bool> CanOpenWhatsApp()
        {
            try
            {
                var uri = new Uri("whatsapp://");
                return await Launcher.CanOpenAsync(uri);
            }
            catch (Exception)
            {
                return false;
            }
        }



    }
}