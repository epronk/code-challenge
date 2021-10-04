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
            if (Environment.NewLine == "\n\r")
                return s;
            else
                return s.Replace("\n\r", Environment.NewLine);
        }
    }
}
