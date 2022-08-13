using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ButtPillowCDS
{
    public class Program
    {

        public enum IceCream
        {
            MintChip,
            Chocolate,
            NoIceCream

        }

        IceCream iceCream;

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void IceCreamMethod(IceCream iceCream)
        {
            if (iceCream == Program.IceCream.MintChip)
            {
                GetIceCream(Program.IceCream.MintChip);

            }
            else if (iceCream == Program.IceCream.Chocolate)
            {
                GetIceCream(Program.IceCream.Chocolate);
            }
            else
            {
                GetIceCream(Program.IceCream.NoIceCream);
            }
        }

        private static void GetIceCream(IceCream iceCream)
        {
            ///Fake method to illustrate the point
        }
    }
}
