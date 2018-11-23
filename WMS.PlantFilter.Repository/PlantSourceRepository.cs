using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.PlantFilter.Data;
using WMS.PlantFilter.IRepository;

namespace WMS.PlantFilter.Repository
{
    public class PlantSourceRepository : IPlantSourceRepository
    {
        private readonly IDapperPlusDB _dapperPlusDB;
        public PlantSourceRepository(IDapperPlusDB dapperPlusDB) 
        {
            this._dapperPlusDB = dapperPlusDB;
        }

        public IEnumerable<PlantSource> QueryPlants(string name=null)
        {
            string sqlAdd = " and plant.plant_code not in(select plant_code from rel_plant_web where web=1)";
            if (!string.IsNullOrWhiteSpace(name))
                sqlAdd += string.Format(@" and plant.plant_name like '%{0}%'", name);

            var sql = string.Format(@"select * from (select b.plant_code, 
               nvl(b.plant_short_name, b.plant_full_name) display_name,
               b.plant_code || '-' || nvl(b.plant_short_name, b.plant_full_name) plant_name
         from bas_plant b  WHERE 1=1 
         order by b.plant_code ) plant where 1=1 {0} ", sqlAdd);

            return _dapperPlusDB.Query<PlantSource>(sql);
        }
    }
}
