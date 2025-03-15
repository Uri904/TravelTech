using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelTech.Views.Gastos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActualizarGasto : ContentPage
    {
        public ActualizarGasto()
        {
            InitializeComponent();
        }


        // -- Navegación -- //

        //Evento del boton btn_Regresar
        private async void btn_Regresar(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();// Regresa a la página anterior
        }

        //Evento del boton BtnCancelar
        private async void BtnCancelar(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Views.Gastos.VerGastos());
        }





    }
}