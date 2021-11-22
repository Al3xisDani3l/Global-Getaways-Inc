using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GG.Core
{
    public interface IRecommender<Tkey,TItem> where Tkey: IEquatable<Tkey> where TItem: IRemarkableItem
    {

        public ICollection<TItem> PredictForUser(Tkey userId);

        public ModelOutput Predict(TItem input, Tkey userId);

        public ICollection<TItem> Predict(ICollection<TItem> travelPackages, Tkey userId);

        public Task TrainModelAsync();

        


    }


    public interface IRecommender: IRecommender<string,PrivateTravelPackage>
    {

    }
}
