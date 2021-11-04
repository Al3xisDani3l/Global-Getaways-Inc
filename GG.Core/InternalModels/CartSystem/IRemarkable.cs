using System;
using System.Collections.Generic;
using System.Text;

namespace GG.Core
{
    public interface IRemarkable:IEntity
    {
        public string Review { get; set; }

        public decimal Price { get; set; }
    }
}
