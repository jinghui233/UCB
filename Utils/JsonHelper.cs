using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class JsonHelper
    {
        public static string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        public static T JsonTo<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
        public static T JsonTo<T>(byte[] bytes)
        {
            return JsonTo<T>(Encoding.UTF8.GetString(bytes));
        }
    }
}
