using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class CollectionsHelper
    {
        public static List<object> Copy(IEnumerable values)
        {
            List<object> list = new List<object>();
            foreach (var item in values)
            {
                list.Add(item);
            }
            return list;
        }
    }
}
