using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QwLua;

namespace QwLuaHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var runtime = LuaFactory.CreateRuntime())
            {
                for (int i = 0; i < 10; i++)
                {
                    runtime.LoadScript(@"func main()
end
func main2()
end
func main1()
end

main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
main1()
");
                }
            }
        }
    }
}
