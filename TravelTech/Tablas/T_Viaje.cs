using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace TravelTech.Tablas
{
    public class T_Viaje
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(255)]
        public string nombre { get; set; }

        [MaxLength(255)]
        public string Fecha_inicio { get; set; }

        [MaxLength(255)]
        public string Fecha_fin { get; set; }

        [MaxLength(255)]
        public string Presupuesto { get; set; }

        // Relaciones con otras tablas
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<T_Destino> Destino { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<T_Gasto> Gasto { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<T_Actividad> Actividad { get; set; }
    }
}