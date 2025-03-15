using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using SQLite;
using System.IO;
using SQLIteBiblioteca.Droid;
using SQLIteBiblioteca.Datos;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteDB))]
namespace SQLIteBiblioteca.Droid
{
    internal class SQLiteDB : ISQLiteDB
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var ruta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(ruta, "TravelTech.db3");
            return new SQLiteAsyncConnection(path);
        }
    }
}