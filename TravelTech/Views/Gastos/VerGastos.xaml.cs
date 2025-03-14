﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelTech.Views.Gastos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerGastos : ContentPage
    {
        public VerGastos()
        {
            InitializeComponent();
        }
        // Evento del Boton btn_Home
        private async void btn_Home(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        // Evento del Boton btn_Detalles
        private async void btn_Detalles(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Viajes.Detalles());
        }

        // Evento del Boton btn_Destinos
        private async void btn_Destinos(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Destinos.VerDestinos());
        }

        // Evento del Boton btn_Actividades
        private async void btn_Actividades(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ActividadesDestinos.VerActividad());
        }

        private async void BtnAgregar(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Views.Gastos.AgregarGasto());
        }
        private async void BtnActualizar(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Views.Gastos.ActualizarGasto());
        }
    }
}