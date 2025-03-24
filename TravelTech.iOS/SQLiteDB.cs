using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

using SQLite;
using SQLiteDemo.iOS;
using SQLIteBitDev.Datos;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteDB))]
namespace SQLiteDemo.iOS
{
    public class SQLiteDB : ISQLiteDB
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var ruta = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(ruta, "TravelTech.db3");
            return new SQLiteAsyncConnection(path);
        }
    
    }
}