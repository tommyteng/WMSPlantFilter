using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WMS.PlantFilter.Contract;
using WMS.PlantFilter.Core.Caching;
using WMS.PlantFilter.Data;
using WMS.PlantFilter.Interceptor;
using WMS.PlantFilter.IRepository;

namespace WMS.PlantFilter.WebServer
{
    public class RelPlantWebServiceImp : RelPlantWebService.Iface
    {
        private readonly IRelPlantWebRepository _relPlantWebRepository;
        private readonly IDapperPlusDB _dapperPlusDB;
        private readonly ICacheManager _cacheManager;

        public RelPlantWebServiceImp(IRelPlantWebRepository relPlantWebRepository, IDapperPlusDB dapperPlusDB
            ,ICacheManager cacheManager)
        {
            this._relPlantWebRepository = relPlantWebRepository;
            this._dapperPlusDB = dapperPlusDB;
            this._cacheManager = cacheManager;
        }
        //[TransactionCallHandlerAttribute]
        public virtual InvokeResult SaveRelPlantWeb(List<RelPlantWeb> rels)
        {
            ResponseCode responseCode;
            string responseMessage;

            IDbTransaction transaction = null;
            IDbConnection conn = null;

            try
            {
                conn = _dapperPlusDB.GetConnection();
                conn.Open();
                transaction = conn.BeginTransaction();

                var newlist = Mapper.Map<List<rel_plant_web>>(rels);
                var extistList = this._relPlantWebRepository.GetAllRelPlantWeb().ToList();

                var addlist = newlist.Except(extistList, new Comparers()).ToList();
                var dellist = extistList.Except(newlist, new Comparers()).ToList();

                addlist.ForEach(item => this._relPlantWebRepository.AddRelPlantWeb(item, _dapperPlusDB, transaction));
                dellist.ForEach(item => this._relPlantWebRepository.DeleteRelPlantWebById(item.rel_id, _dapperPlusDB, transaction));

                transaction.Commit();

                responseCode = ResponseCode.SUCCESS;
                responseMessage = "保存成功！" ;

                if (this._cacheManager.IsSet("RelPlantWebs_Cache"))
                    this._cacheManager.Remove("RelPlantWebs_Cache");

                if (this._cacheManager.IsSet("Plants_Cache"))
                    this._cacheManager.Remove("Plants_Cache");
            }
            catch (Exception ex)
            {
                responseCode = ResponseCode.FAILED;
                responseMessage = "保存失败！原因：" + ex.Message;

                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
            finally {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return new InvokeResult
            {
                Code = responseCode,
                Message = responseMessage
            };
        }

        public List<RelPlantWeb> QueryRelPlantWebs()
        {
            return this._cacheManager.Get<List<RelPlantWeb>>("RelPlantWebs_Cache", () =>
               Mapper.Map<List<RelPlantWeb>>(this._relPlantWebRepository.GetAllRelPlantWeb()));
        }
    }

    public class Comparers : IEqualityComparer<rel_plant_web>
    {
        public bool Equals(rel_plant_web p1, rel_plant_web p2)
        {
            return p1.plant_code == p2.plant_code && p1.web == p2.web;
        }

        public int GetHashCode(rel_plant_web obj)
        {
           return base.GetHashCode();
        }
    }
  
}