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
	public partial class ActualizarDestino : ContentPage
	{
		public ActualizarDestino ()
		{
			InitializeComponent ();
		}
        private async void BtnCancelar(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Views.Destinos.VerDestinos());
        }
    }
}