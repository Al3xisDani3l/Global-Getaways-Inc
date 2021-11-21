using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GG.Core
{
    public interface IRecommender<Tkey> where Tkey: IEquatable<Tkey>
    {

        public Task<IList<IRemarkableItem>> GetBestItems(Tkey key);

        public Task<double> TrainModel();

        


    }


    public interface IRecommender: IRecommender<string>
    {

    }
}
