using GG.Core;
using GG.Data;
using System.Linq;
using System.Collections.Generic;
using Microsoft.ML;
using AutoMapper;
using Microsoft.ML.Data;
using System.Threading.Tasks;
using System.IO;
using System;
using Microsoft.Extensions.Logging;

namespace GG.WebPageMVC
{
    public class RecommenderSystem:IRecommender
    {

        readonly ApplicationDbContext _dbcontex;
        readonly IMapper _mapper;
        public Lazy<PredictionEngine<RatingModelImput, ModelOutput>> _predictEngine;
        private  MLContext _mlContext;
        private ILogger<RecommenderSystem> _logger;
       

       

        private static string MLNetModelPath = Path.GetFullPath(@"wwwroot\RS\GGMLModelTrainer.zip");


       

        public RecommenderSystem(ApplicationDbContext contex, IMapper mapper, ILogger<RecommenderSystem> logger)
        {
            _dbcontex = contex;
            _mapper = mapper;
            _logger = logger;

            _mlContext = new MLContext();
            ITransformer mlModel = _mlContext.Model.Load(MLNetModelPath, out var _);
           
            _predictEngine = new Lazy<PredictionEngine<RatingModelImput, ModelOutput>>(()=> CreatePredictEngine(mlModel), true);
            _logger.LogInformation("Recommender System cargado exitosamente!");
        }

      


        public  ModelOutput Predict(PrivateTravelPackage input, string userId)
        {

            var model = new RatingModelImput()
            {
                Id = 1L,
                UserId = userId,
                TravelPackageId = input.Id
            };

            var predEngine = _predictEngine.Value;
            return predEngine.Predict(model);
        }


        public  ICollection<PrivateTravelPackage> Predict(ICollection<PrivateTravelPackage> travelPackages, string userId)
        {
            
            foreach (var item in travelPackages)
            {
                item.RatingPrediction = Predict(item, userId).Score;
               
                    
                    
                       
                
            }
            int count = 0;
            foreach (var item in travelPackages.OrderByDescending(p => p.RatingPrediction).Take(5))
            {
                count++;
                _logger.LogInformation("Prediccion {0} | User: {1} con el paquete: {2} | Prediccion: {3}", count, userId, item.Id, item.RatingPrediction);
            }

            return travelPackages.OrderByDescending(p => p.RatingPrediction).ToList();

        }


         public ICollection<PrivateTravelPackage> PredictForUser(string userId)
        {
            var packages = _dbcontex.TravelPackages.ToList();

             return  Predict(packages, userId);
        }

        public  ITransformer RetrainPipeline(MLContext context, IDataView trainData)
        {


            var pipeline = BuildPipeline(context);
            var model = pipeline.Fit(trainData);

            return model;
        }

        /// <summary>
        /// build the pipeline that is used from model builder. Use this function to retrain model.
        /// </summary>
        /// <param name="mlContext"></param>
        /// <returns></returns>
        private IEstimator<ITransformer> BuildPipeline(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations
            var pipeline = mlContext.Transforms.Conversion.MapValueToKey(@"UserId", @"UserId")
                                    .Append(mlContext.Transforms.Conversion.MapValueToKey(@"TravelPackageId", @"TravelPackageId"))
                                    .Append(mlContext.Recommendation().Trainers.MatrixFactorization(labelColumnName: @"Punctuation", matrixColumnIndexColumnName: @"UserId", matrixRowIndexColumnName: @"TravelPackageId"));

            return pipeline;
        }

        public  IDataView LoadData(MLContext mLContext)
        {
            var data = _dbcontex.Ratings.ToList();
            IEnumerable<RatingModelImput> ratings = _mapper.Map<List<RatingModelImput>>(data);

            return mLContext.Data.LoadFromEnumerable<RatingModelImput>(ratings);

        }


        private  PredictionEngine<RatingModelImput, ModelOutput> CreatePredictEngine(ITransformer transformer)
        {
            
            return _mlContext.Model.CreatePredictionEngine<RatingModelImput, ModelOutput>(transformer);
        }

        public async Task TrainModelAsync()
        {
            await Task.Run(() =>
             {

                 try
                 {
                     _logger.LogInformation("Iniciando el entrenamiento del model!");
                     var newData = LoadData(_mlContext);
                     var result = RetrainPipeline(_mlContext, newData);
                     _mlContext.Model.Save(result, newData.Schema, MLNetModelPath);
                     _predictEngine = new Lazy<PredictionEngine<RatingModelImput, ModelOutput>>(() => CreatePredictEngine(result), true);
                     _logger.LogInformation("El modelo fue entrenado exitosamente!");
                 }
                 catch (Exception ex)
                 {

                     _logger.LogError(ex, "Ups algo ha acurrido!, el modelo no pudo acompletar su entrenamiento:(");
                 }
             });
        }
    }

 
}
