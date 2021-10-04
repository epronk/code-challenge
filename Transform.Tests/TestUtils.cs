using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail.Tests
{
    public class TestUtils
    {
        public static string Normalize(string s)
        {
            if (Environment.NewLine == "\r\n")
                return s;
            else
                return s.Replace("\r\n", Environment.NewLine);
        }
    }
}
