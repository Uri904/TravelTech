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
    public partial class AgregarGasto : ContentPage
    {
        public AgregarGasto()
        {
            InitializeComponent();
        }
        private async void BtnCancelar(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Views.Gastos.VerGastos());
        }
    }
}