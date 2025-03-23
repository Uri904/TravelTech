using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;
using TravelTech.Tablas;

namespace TravelTech.Tablas
{
    public class T_Recordatorio
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(255)]
        public string PK_id_recordatorio { get; set; }

        [MaxLength(255)]
        public string Fecha_recordatorio { get; set; }

        [MaxLength(255)]
        public string Mensaje { get; set; }

        [MaxLength(255)]
        public string estado { get; set; }

        [MaxLength(255)]
        public string importancia { get; set; }

        [ForeignKey(typeof(T_Viaje))] // Relación con T_Viaje
        public int FK_id_viaje { get; set; }

        [ForeignKey(typeof(T_Actividad))]
        public int FK_id_actividad { get; set; }

        // Relación con Actividades
        [ManyToOne]
        public T_Actividad Actividad { get; set; }

        // Relación con Viaje (si es necesario)
        [ManyToOne]
        public T_Viaje Viaje { get; set; }
    }
}