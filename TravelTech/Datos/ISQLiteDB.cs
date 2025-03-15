using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace SQLIteBiblioteca.Datos
{
    public interface ISQLiteDB
    {
        SQLiteAsyncConnection GetConnection();
    }
}
