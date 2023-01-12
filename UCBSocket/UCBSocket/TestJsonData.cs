using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace UCBSocket
{
    public class TestJsonData
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime dateTime { get; set; }
        InnerJsonData innerJsonData { get; set; }
        public static TestJsonData GetRandomData()
        {
            TestJsonData data = new TestJsonData();
            data.id = Number.random.Next(1, 999);
            data.name = Number.random.Next(999, 999999).ToString();
            data.dateTime = DateTime.Now;
            data.innerJsonData = InnerJsonData.GetRandomData();
            return data;
        }
        public static List<TestJsonData> GetRandomDatas(int count)
        {
            List<TestJsonData> data = new List<TestJsonData>();
            for (int i = 0; i < count; i++)
            {
                data.Add(GetRandomData());
            }
            return data;
        }
    }
    public class InnerJsonData
    {
        public int count { get; set; }
        public string address { get; set; }
        public double number { get; set; }
        public DateTime curTime { get; set; }
        public static InnerJsonData GetRandomData()
        {
            InnerJsonData data = new InnerJsonData();
            data.count = Number.random.Next(1, 999);
            data.address = Number.random.Next(999, 999999).ToString();
            data.number = Number.random.NextDouble();
            data.curTime = DateTime.Now;
            return data;
        }
    }
}
