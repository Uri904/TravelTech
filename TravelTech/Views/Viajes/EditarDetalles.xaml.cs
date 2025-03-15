using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelTech.Views.Viajes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditarDetalles : ContentPage
    {
        public EditarDetalles()
        {
            InitializeComponent();
        }

        // -- Navegación -- //

        //Evento del btn_Regresar
        private async void btn_Regresar(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();// Regresa a la página anterior
        }




    }
}