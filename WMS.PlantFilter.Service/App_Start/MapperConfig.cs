using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMS.PlantFilter.WebServer
{
    public class MapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
               {
                   cfg.CreateMap<WMS.PlantFilter.Data.PlantSource, WMS.PlantFilter.Contract.PlantSource>()
                      .ForMember(dto => dto.Display_name, opt => opt.MapFrom(m => m.display_name))
                      .ForMember(dto => dto.Plant_code, opt => opt.MapFrom(m => m.plant_code))
                      .ForMember(dto => dto.Plant_name, opt => opt.MapFrom(m => m.plant_name));

                   cfg.CreateMap<WMS.PlantFilter.Data.rel_plant_web, WMS.PlantFilter.Contract.RelPlantWeb>()
                      .ForMember(dto => dto.Plant_code, opt => opt.MapFrom(m => m.plant_code))
                      .ForMember(dto => dto.Plant_name, opt => opt.MapFrom(m => m.plant_name))
                      .ForMember(dto => dto.Web, opt => opt.MapFrom(m => m.web));

                   cfg.CreateMap<WMS.PlantFilter.Contract.RelPlantWeb,WMS.PlantFilter.Data.rel_plant_web>()
                      .ForMember(dto => dto.plant_code, opt => opt.MapFrom(m => m.Plant_code))
                      .ForMember(dto => dto.plant_name, opt => opt.MapFrom(m => m.Plant_name))
                      .ForMember(dto => dto.web, opt => opt.MapFrom(m => m.Web));
               });
        }
    }
}