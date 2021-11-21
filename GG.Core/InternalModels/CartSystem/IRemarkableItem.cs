using System;
using System.Collections.Generic;
using System.Text;

namespace GG.Core
{



    public interface IRemarkableItem:IEntity
    {
        public string Review { get; set; }

        public decimal Price { get; set; }
    }



}
