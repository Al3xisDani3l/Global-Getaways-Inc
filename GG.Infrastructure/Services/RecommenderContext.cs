using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GG.Core;
using GG.Data;
namespace GG.Infrastructure
{
    public class RecommenderContext : IRecommender
    {

        private readonly ApplicationDbContext _dbContext;

        public RecommenderContext(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<IList<IRemarkableItem>> GetBestItems(string key)
        {

          

            throw new NotImplementedException();
        }

        public Task<double> TrainModel()
        {
            throw new NotImplementedException();
        }
    }
}
