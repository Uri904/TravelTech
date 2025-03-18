using System;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace TravelTech.Tablas
{
    public class T_Gasto
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(T_Viaje))]
        public int PK_id_viaje { get; set; }

        [MaxLength(255)]
        public string Categoria { get; set; }

        public double Monto { get; set; }

        public DateTime Fecha_gasto { get; set; } // Cambio a DateTime

        // Relación con Viaje
        [ManyToOne]
        public T_Viaje Viaje { get; set; }
    }
}
