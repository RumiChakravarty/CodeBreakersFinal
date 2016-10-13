using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Code_BreakersEventBudget
{
    public class WalmartApi
    {
        public List<WalmartItem> search(string query)
            {
            WebRequest request = WebRequest.Create("http://api.walmartlabs.com/v1/search?query=ipod&format=json&apiKey=hf4pbpg3p2e52vw6nfhj8chr");
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();

            string value;
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                value = reader.ReadToEnd();
            }

            JObject Jobject = JObject.Parse(value);
            JArray items = (JArray)Jobject.GetValue("items");

            List<WalmartItem> ItemReturn = new List<WalmartItem>();

            foreach (JObject item in items)
            {
                WalmartItem AItem = new WalmartItem();
                AItem.Name = (string)item.GetValue("name");
                AItem.Name = (string)item.GetValue("salePrice");
                AItem.Name = (string)item.GetValue("longDescription");

            }
            return ItemReturn;


            }

    }
}