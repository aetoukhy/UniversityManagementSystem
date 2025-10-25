using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using UMS.Universities;
using UMS.Colleges;

namespace UMS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Data.Initialize();

            Navigation.HomePage();

        }
    }
}
