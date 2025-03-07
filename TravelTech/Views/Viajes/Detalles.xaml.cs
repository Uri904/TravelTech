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
    public partial class Detalles : ContentPage
    {
        public Detalles()
        {
            InitializeComponent();
        }
        //Evento btn_VerDetalles
        private async void btn_Destino(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Destinos.VerDestinos());
        }

        //Evento btn_EditarDetalles
        private async void btn_EditarDetalles(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Viajes.EditarDetalles());
        }

    }
}