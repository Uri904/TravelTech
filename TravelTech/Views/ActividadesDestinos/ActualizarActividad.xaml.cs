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
	public partial class ActualizarActividad : ContentPage
	{
		public ActualizarActividad ()
		{
			InitializeComponent ();
		}
        private async void BtnCancelar(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Views.ActividadesDestinos.VerActividad());
        }
    }
}