namespace csharp WMS.PlantFilter.Contract
namespace java com.aaa.bbb.Contract

service PlantSourceService{
	list<PlantSource> QueryPlants(1:string name)
}

service RelPlantWebService{
	InvokeResult SaveRelPlantWeb(1:list<RelPlantWeb> rels)

	list<RelPlantWeb> QueryRelPlantWebs()
}

enum ResponseCode {  
  SUCCESS = 0,  
  FAILED = 1,  
}

struct RelPlantWeb{
    1: required string plant_code;
	2: optional string plant_name;
    3: required i32 web;
}

struct PlantSource {
    1: required string plant_code;
    2: required string display_name;
	3: required string plant_name
}

struct InvokeResult {
    1: required ResponseCode code;
    2: optional string Message;
}