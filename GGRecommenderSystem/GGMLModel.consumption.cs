﻿// This file was auto-generated by ML.NET Model Builder. 
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
public partial class GGMLModel
{
    /// <summary>
    /// model input class for GGMLModel.
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
    /// model output class for GGMLModel.
    /// </summary>
    #region model output class
    public class ModelOutput
    {
        public float Score { get; set; }
    }
    #endregion

    private static string MLNetModelPath = Path.GetFullPath("GGMLModel.zip");

    public static readonly Lazy<PredictionEngine<ModelInput, ModelOutput>> PredictEngine = new Lazy<PredictionEngine<ModelInput, ModelOutput>>(() => CreatePredictEngine(), true);

    /// <summary>
    /// Use this method to predict on <see cref="ModelInput"/>.
    /// </summary>
    /// <param name="input">model input.</param>
    /// <returns><seealso cref=" ModelOutput"/></returns>
    public static ModelOutput Predict(ModelInput input)
    {
        var predEngine = PredictEngine.Value;
        return predEngine.Predict(input);
    }

    private static PredictionEngine<ModelInput, ModelOutput> CreatePredictEngine()
    {
        var mlContext = new MLContext();
        ITransformer mlModel = mlContext.Model.Load(MLNetModelPath, out var _);
        return mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
    }
}
