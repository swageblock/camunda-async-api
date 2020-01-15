using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncPoweredAPI.Models
{
    public class Data
    {
        private static readonly List<string> values = new List<string>();
        public static List<string> Values
        {
            get
            {
                return values;
            }
        }
    }
}
