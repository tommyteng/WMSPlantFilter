using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.PlantFilter.Data;

namespace WMS.PlantFilter.IRepository
{
    public interface IPlantSourceRepository
    {
        IEnumerable<PlantSource> QueryPlants(string name = null);
    }
}
