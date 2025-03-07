using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelTech.Views.Destinos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Crear : ContentPage
    {
        public Crear()
        {
            InitializeComponent();
        }

        //Evento del boton btn_Regresar
        private async void btn_Regresar(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();// Regresa a la página anterior
        }

    }
}