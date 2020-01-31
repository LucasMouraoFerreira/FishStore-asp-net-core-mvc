using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishStore.Data
{
    public interface IDbInitializer
    {
        void initialize();
    }
}
