using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;
using TravelTech.Tablas;

namespace TravelTech.Tablas
{
    public class T_Actividad
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(255)]
        public string PK_id_actividad { get; set; }

        [MaxLength(255)]
        public string nombre { get; set; }

        [MaxLength(255)]
        public string Fecha_actividad { get; set; }

        [MaxLength(255)]
        public string nota { get; set; }

        [ForeignKey(typeof(T_Viaje))]
        public int PK_id_viaje { get; set; }

        // Relación con Viajes
        [ManyToOne]
        public T_Viaje Viaje { get; set; }

        // Relación con Recordatorios
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<T_Recordatorio> Recordatorio { get; set; }
    }
}