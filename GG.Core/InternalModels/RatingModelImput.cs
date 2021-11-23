/*
Nombre: Daniel Emiliano Burrola Avalos
Fecha: 16/11/2021
Funcionalidad: Se implementara la entrada del modelo de Rating
*/

using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace GG.Core
{
    /// <summary>
    /// Entrada del modelo de Rating con los datos de puntuacion, comentarios, Usuario, etc.
    /// </summary>
     public class RatingModelImput
    {

        [ColumnName(@"Id")]
        public float Id { get; set; }

        [ColumnName(@"UserId")]
        public string UserId { get; set; }

        [ColumnName(@"TravelPackageId")]
        public float TravelPackageId { get; set; }

        [ColumnName(@"Punctuation")]
        public float Punctuation { get; set; }

        [ColumnName(@"Comment")]
        public string Comment { get; set; }

        [ColumnName(@"PostingDate")]
        public string PostingDate { get; set; }

        [ColumnName(@"LastUpdate")]
        public string LastUpdate { get; set; }

        [ColumnName(@"Guid")]
        public string Guid { get; set; }
    }
}
