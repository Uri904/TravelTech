﻿using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;
using TravelTech.Tablas;

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

        [MaxLength(255)]
        public string monto { get; set; }

        [MaxLength(255)]
        public string Fecha_gasto { get; set; }

        // Relación con Viaje
        [ManyToOne]
        public T_Viaje Viaje { get; set; }
    }
}