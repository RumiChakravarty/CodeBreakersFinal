using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EasyHttp.Http;

namespace Code_BreakersEventBudget.Utility
{
    public class Helper
    {
        public List<ListItem> CallWalmartAPI(string strSearchName, int uId, int lId)
        {
            //string url = "http://api.walmartlabs.com/v1/search?query=ipod&format=json&apiKey=hf4pbpg3p2e52vw6nfhj8chr";
            string url = "http://api.walmartlabs.com/v1/search?query=SEARCH_NAME&format=json&apiKey=hf4pbpg3p2e52vw6nfhj8chr";
            url = url.Replace("SEARCH_NAME", strSearchName);
            string strJson = string.Empty;
            var http = new HttpClient();
            http.Request.Accept = HttpContentTypes.ApplicationJson;
            var response = http.Get(url);
            var body = response.DynamicBody;
            //parse Json response
            /*
            body.query
            "ipod"
            body.items[0].itemid
            body.items[0].itemId
            50689211
            body.numItems
            10
            */
            string sQuery = body.query;
            int iNumItems = Convert.ToInt32(body.numItems);
            List<ListItem> listItems = new List<ListItem>();
            for (int i = 0; i < Convert.ToInt32(body.numItems); i++)
            {
                ListItem listItem = new ListItem();
                listItem.ListID = lId;
                listItem.UserID = uId;
                listItem.ProductName = body.items[i].name;
                listItem.Price = Convert.ToDecimal(body.items[i].salePrice);
                listItem.ThumbnailUrl = body.items[i].thumbnailImage;
                //listItem.ProductURL = body.items[i].productUrl;
                listItems.Add(listItem);
            }
            return (listItems);

        }

    }
}