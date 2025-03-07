using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelTech.Views.ActividadesDestinos
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class VerActividad : ContentPage
	{
		public VerActividad ()
		{
			InitializeComponent ();
		}

        //Evento del botón btn_CrearActividad
        private async void btn_CrearActividad(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ActividadesDestinos.CrearActividad());
        }

        private void ToggleActividad1(object sender, System.EventArgs e)
        {
            contentActividad1.IsVisible = !contentActividad1.IsVisible;
            btnActividad1.Text = contentActividad1.IsVisible ? "Subir a la Torre Eiffel ▲" : "Subir a la Torre Eiffel ▼";
        }

        private void ToggleActividad2(object sender, System.EventArgs e)
        {
            contentActividad2.IsVisible = !contentActividad2.IsVisible;
            btnActividad2.Text = contentActividad2.IsVisible ? "Recorrer Montmartre ▲" : "Recorrer Montmartre ▼";
        }

    }
}