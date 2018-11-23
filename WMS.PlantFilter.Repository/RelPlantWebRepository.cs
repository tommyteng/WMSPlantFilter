using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.PlantFilter.Data;
using WMS.PlantFilter.IRepository;

namespace WMS.PlantFilter.Repository
{
    public class RelPlantWebRepository : IRelPlantWebRepository
    {
        private readonly IDapperPlusDB _dapperPlusDB;
        public RelPlantWebRepository(IDapperPlusDB dapperPlusDB) 
        {
            this._dapperPlusDB = dapperPlusDB;
        }

        /// <summary>
        /// 增加映射信息
        /// </summary>
        /// <param name="rel"></param>
        /// <returns></returns>
        public bool AddRelPlantWeb(rel_plant_web rel, IDapperPlusDB dapperPlusDB,IDbTransaction transaction)
        {
            var sql = @"INSERT INTO rel_plant_web(plant_code,web) VALUES(:plant_code,:web)";
            if (dapperPlusDB != null)
                return dapperPlusDB.Execute(sql, new { rel.plant_code, rel.web }, transaction) > 0;
            else
                return _dapperPlusDB.Execute(sql, new { rel.plant_code, rel.web }) > 0;
        }
        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteRelPlantWebById(int id, IDapperPlusDB dapperPlusDB,IDbTransaction transaction)
        {
            var sql = @"Delete rel_plant_web WHERE rel_id=:rel_id";
            if (dapperPlusDB != null)
                return dapperPlusDB.Execute(sql, new { rel_id = id }, transaction) > 0;
            else
                return _dapperPlusDB.Execute(sql, new { rel_id = id }) > 0;
        }
        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="rel"></param>
        /// <returns></returns>
        public bool ModifyRelPlantWeb(rel_plant_web rel)
        {
            var sql = @"UPDATE rel_plant_web SET plant_code = :plant_code ,web = :web     
               WHERE rel_id=@rel_id";
            return _dapperPlusDB.Execute(sql, rel) > 0;
        }
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public rel_plant_web GetRelPlantWebById(string pland,int web)
        {
            var sql = @"SELECT a.rel_id,a.web, a.plant_code, b.plant_code || '-' || nvl(b.plant_short_name, b.plant_full_name) plant_name 
                      FROM REL_PLANT_WEB a  left join bas_plant b
                       on a.plant_code= b.plant_code where 1=1 and a.plant_code=:plant_code and a.web=:web";
            return _dapperPlusDB.Query<rel_plant_web>(sql, new { plant_code = pland, web = web }).FirstOrDefault();
        }
        /// <summary>
        /// 获取所有对象
        /// </summary>
        /// <returns></returns>
        public IEnumerable<rel_plant_web> GetAllRelPlantWeb()
        {
            var sql = @" SELECT a.rel_id,a.web, a.plant_code, b.plant_code || '-' || nvl(b.plant_short_name, b.plant_full_name) plant_name 
                      FROM REL_PLANT_WEB a  left join bas_plant b
                       on a.plant_code= b.plant_code where 1=1 ";

            return _dapperPlusDB.Query<rel_plant_web>(sql);
        }
    }
}
