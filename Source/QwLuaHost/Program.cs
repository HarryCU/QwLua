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

        public void CreateNamed()
        {

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
end
");

                runtime.Execute("Main", "dd");

            }
        }
    }
}
