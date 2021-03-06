/*
 Nombre: Daniel Emiliano Burrola Avalos
 Fecha: 14/11/2021
 Funcionalidad: Se generara el Modelo de Consumo para el Recommender System a través del análisis de datos de nuestros clientes
 */
// This file was auto-generated by ML.NET Model Builder. 
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using GG.Core;
using System.Threading.Tasks;

namespace GG.WebPageMVC
{
    public partial class GGMLModel
    {
        /// <summary>
        /// Clase de entrada del modelo para GGMLModel.
        /// </summary>
        #region model input class
        public class ModelInput
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

        #endregion

        /// <summary>
        /// Clase de salida del modelo para GGMLModel.
        /// </summary>
        #region model output class
     
        #endregion

        private static string MLNetModelPath = Path.GetFullPath(@"wwwroot\RS\GGMLModelTrainer.zip");

        public static readonly Lazy<PredictionEngine<ModelInput, ModelOutput>> PredictEngine = new Lazy<PredictionEngine<ModelInput, ModelOutput>>(() => CreatePredictEngine(), true);

        /// <summary>
        /// Se usa este metodo para predecir en <see cref="ModelInput"/>.
        /// </summary>
        /// <param name="input">model input.</param>
        /// <returns><seealso cref=" ModelOutput"/></returns>
        public static ModelOutput Predict(string userId, PrivateTravelPackage input)
        {

            var model = new ModelInput()
            {
                Id = 1L,
                UserId = userId,
                TravelPackageId = input.Id
            };

            var predEngine = PredictEngine.Value;
            return predEngine.Predict(model);
        }

    

        private static PredictionEngine<ModelInput, ModelOutput> CreatePredictEngine()
        {
            var mlContext = new MLContext();
            ITransformer mlModel = mlContext.Model.Load(MLNetModelPath, out var _);
            return mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
        }
    }




}
