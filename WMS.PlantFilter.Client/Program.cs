using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WMS.PlantFilter.Contract;

namespace WMS.PlantFilter.Client
{
    class Program
    {
        //thrift --gen <language> <Thrift filename>
        static void Main(string[] args)
       {

            List<string> ListA = new List<string>() { "123","456","789"};
            List<string> ListB = new List<string>() { "234", "456", "789" };
            List<string> ListResult = new List<string>();

            ListResult = ListA.Distinct().ToList();//去重
            ListResult = ListA.Except(ListB).ToList();//差集
            ListResult = ListA.Union(ListB).ToList();  //并集
            ListResult = ListA.Intersect(ListB).ToList();//交集

            Thread.Sleep(10000);

            string name="";
            try
            {
                var list = ClientProxy.ClientPlantExecute<List<PlantSource>>(c => c.QueryPlants(name));
                var newlist = new List<RelPlantWeb>()
                    {
                        new RelPlantWeb(){ Plant_code="ABJS", Web=1},
                        new RelPlantWeb(){ Plant_code="BLT", Web=1},
                        new RelPlantWeb(){ Plant_code="DB002", Web=1},
                        new RelPlantWeb(){ Plant_code="DB001", Web=1}
                    };
                ClientProxy.ClientRelExecute<InvokeResult>(client => client.SaveRelPlantWeb(newlist));
                var lll = ClientProxy.ClientRelExecute <List<RelPlantWeb>>(client => client.QueryRelPlantWebs());
            }
            catch (Exception ex)
            { 
            
            }
          

            
        }
    }
}
