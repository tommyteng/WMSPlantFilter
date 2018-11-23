using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.PlantFilter.Contract;
using WMS.PlantFilter.Core.Caching;
using WMS.PlantFilter.Interceptor;
using WMS.PlantFilter.IRepository;

namespace WMS.PlantFilter.WebServer
{
    public class PlantSourceServiceImp : PlantSourceService.Iface
    {
        private readonly IPlantSourceRepository _plantSourceRepository;
        private readonly ICacheManager _cacheManager;
        public PlantSourceServiceImp(IPlantSourceRepository plantSourceRepository, ICacheManager cacheManager)
        {
            this._plantSourceRepository = plantSourceRepository;
            this._cacheManager = cacheManager;
        }


        /// <summary>
        /// 查询所有工厂信息
        /// </summary>
        /// <param name="name">根据名称查询 支持模糊查询</param>
        /// <returns></returns>
        public virtual List<PlantSource> QueryPlants(string name)
        {
            var plants = this._cacheManager.Get<List<PlantSource>>("Plants_Cache", () =>
                     Mapper.Map<List<PlantSource>>(this._plantSourceRepository.QueryPlants()));

            if (!string.IsNullOrWhiteSpace(name))
                plants = plants.Where(item => item.Plant_name.Contains(name)).ToList();

            return plants;
        }
    }
}