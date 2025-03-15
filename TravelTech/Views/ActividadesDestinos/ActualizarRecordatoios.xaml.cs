﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelTech.Views.ActividadesDestinos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActualizarRecordatoios : ContentPage
    {
        public ActualizarRecordatoios()
        {
            InitializeComponent();
        }


        // -- Navegación -- // 

        //Evento del boton btn_Regresar
        private async void btn_Regresar(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();// Regresa a la página anterior
        }

        //Evento del boton btn_Cancelar
        private async void btn_Cancelar(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();// Regresa a la página anterior
        }
    }
}