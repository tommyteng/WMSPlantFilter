using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.PlantFilter.Data;

namespace WMS.PlantFilter.IRepository
{
    public interface IRelPlantWebRepository
    {
        bool AddRelPlantWeb(rel_plant_web rel, IDapperPlusDB dapperPlusDB, IDbTransaction transaction);

        bool DeleteRelPlantWebById(int id, IDapperPlusDB dapperPlusDB, IDbTransaction transaction);

        bool ModifyRelPlantWeb(rel_plant_web rel);

        rel_plant_web GetRelPlantWebById(string pland,int web);

        IEnumerable<rel_plant_web> GetAllRelPlantWeb();
    }
}
