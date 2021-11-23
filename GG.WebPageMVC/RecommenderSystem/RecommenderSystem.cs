/*
 Nombre: Daniel Emiliano Burrola Avalos
 Fecha: 21/11/2021
 Funcionalidad: Se creara el Recommender System que recomendara paquetes de turismo
 segun los datos que se obtengan de cada usuario
 */
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
    /// <summary>
    /// Se crea la clase para iniciar el Recommender System
    /// </summary>
    public class RecommenderSystem:IRecommender
    {

        readonly ApplicationDbContext _dbcontex;
        readonly IMapper _mapper;
        public Lazy<PredictionEngine<RatingModelImput, ModelOutput>> _predictEngine;
        private  MLContext _mlContext;
        private ILogger<RecommenderSystem> _logger;
       

       

        private static string MLNetModelPath = Path.GetFullPath(@"wwwroot\RS\GGMLModelTrainer.zip");



       
        /// <summary>
        /// Inicia una nueva instancia de <seealso cref="RecommenderSystem"/>
        /// </summary>
        /// <param name="contex">Una instancia de base de datos</param>
        /// <param name="mapper">Un mapeador</param>
        /// <param name="logger">Un logger</param>
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

      

        /// <summary>
        /// Se genera el modelo de salida que predecira los paquetes que se le recomiendan al usuario
        /// </summary>
        /// <param name="input">Se pide un input</param>
        /// <param name="userId">Se obtiene el Id del usuario</param>
        /// <returns></returns>
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

        /// <summary>
        /// Se predicen los paquetes a recomendar segun el rating de forma descendente para que aparezcan
        /// los mas gustados al inicio
        /// </summary>
        /// <param name="travelPackages"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Se predicen los paquetes de viaje para el usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
         public ICollection<PrivateTravelPackage> PredictForUser(string userId)
        {
            var packages = _dbcontex.TravelPackages.ToList();

             return  Predict(packages, userId);
        }

        /// <summary>
        /// Se utiliza esta función para reentrenar el modelo.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="trainData"></param>
        /// <returns></returns>
        public  ITransformer RetrainPipeline(MLContext context, IDataView trainData)
        {


            var pipeline = BuildPipeline(context);
            var model = pipeline.Fit(trainData);

            return model;
        }

        /// <summary>
        /// Construye la Pipeline que se utiliza desde el generador de modelos.
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
        /// <summary>
        /// Se cargan los datos de ML, orma de crear componentes para la preparación de datos
        /// ,entrenamiento, predicción y evaluación de modelos
        /// </summary>
        /// <param name="mLContext"></param>
        /// <returns></returns>
        public  IDataView LoadData(MLContext mLContext)
        {
            var data = _dbcontex.Ratings.ToList();
            IEnumerable<RatingModelImput> ratings = _mapper.Map<List<RatingModelImput>>(data);

            return mLContext.Data.LoadFromEnumerable<RatingModelImput>(ratings);

        }

        /// <summary>
        /// Se utiliza el modelo de Rating y el Modelo de salida para la prediccion
        /// </summary>
        /// <param name="transformer"></param>
        /// <returns></returns>
        private  PredictionEngine<RatingModelImput, ModelOutput> CreatePredictEngine(ITransformer transformer)
        {
            
            return _mlContext.Model.CreatePredictionEngine<RatingModelImput, ModelOutput>(transformer);
        }
        /// <summary>
        /// Se entrena el modelo con la informacion del LogIn y te avisa si se entrena el modelo 
        /// de manera correcta
        /// </summary>
        /// <returns></returns>
        public async Task TrainModelAsync()
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
            
        }
    }

 
}
