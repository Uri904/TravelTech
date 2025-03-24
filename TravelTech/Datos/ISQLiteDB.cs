using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace SQLIteBitDev.Datos
{
    public interface ISQLiteDB
    {
        SQLiteAsyncConnection GetConnection();
    }
}
