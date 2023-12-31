﻿using CsvHelper.Configuration.Attributes;
using System;

namespace SFA2Graph.Converter.Models
{
    internal class Arc
    {
        #region Public Properties

        [Index(0)]
        public int ArcID { get; set; }

        [Index(1)]
        public string FromX { get; set; }

        [Index(2)]
        public string FromY { get; set; }

        [Index(5)]
        public string Length { get; set; }

        [Index(8)]
        public int Level { get; set; }

        [Index(9)]
        public int? MaxSpeed { get; set; }

        [Index(10)]
        public int? NoTurnFrom { get; set; }

        [Index(11)]
        public int? NoTurnTo { get; set; }

        [Index(15)]
        [Name("ORT")]
        public string Ort { get; set; }

        [Index(16)]
        [Name("ORTSTEIL")]
        public string Ortsteil { get; set; }

        [Index(7)]
        public string RoadBlocking { get; set; }

        [Index(6)]
        public int RoadClass { get; set; }

        [Index(14)]
        [Name("ROUTENUMBER")]
        public int? RouteNumber { get; set; }

        [Index(13)]
        [Name("STRASSE")]
        public string Strasse { get; set; }

        [Index(3)]
        public string ToX { get; set; }

        [Index(4)]
        public string ToY { get; set; }

        [Index(17)]
        [Name("TYP")]
        public int Typ { get; set; }

        [Index(18)]
        [Name("TYPNAME")]
        public string TypName { get; set; }

        [Index(12)]
        public string Vertices { get; set; }

        #endregion Public Properties
    }
}