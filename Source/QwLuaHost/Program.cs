using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QwLua;

namespace QwLuaHost
{
    class NpoiService
    {
        public NpoiService(string fileName)
        {

        }

        public int Length { get; set; }
        public int Count { get; set; }

        public void CreateNamed()
        {

        }

        public object Next()
        {
            return null;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            using (var runtime = LuaFactory.CreateRuntime())
            {
                runtime.TypeRegister(typeof(NpoiService));

                runtime.LoadScript(@"
func Main(fileName)
    var service = NpoiService(fileName)
    service:CreateNamed()
    var i = service.Length
    var c = service.Count
    while( i <= c )
    do
        var row = service:Next()

    end
end
");

                runtime.Execute("Main", "dd");

            }
        }
    }
}
