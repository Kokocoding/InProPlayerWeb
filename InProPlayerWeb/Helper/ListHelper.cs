using System.Text.Json.Nodes;

namespace InProPlayerWeb.Helper
{
    public class ListHelper
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public string ControllerName;

        public ListHelper(IWebHostEnvironment hostingEnvironment) 
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public JsonNode GetJsonData() 
        {
            string jsonfile = Path.Combine(_hostingEnvironment.ContentRootPath, "Views");
            string jsonString = File.ReadAllText($"{jsonfile}\\{ControllerName}\\{ControllerName}.json");
            return JsonNode.Parse(jsonString);

            string jsfn = HostingEnvironment.MapPath(string.Format("/Areas/{0}/{1}.json", PageArea, PageTable));
            string jsStr = System.IO.File.ReadAllText(jsfn);
            JObject jo = JObject.Parse(jsStr);
            opList = (JObject)jo["opListStr"];
            fieldsList = (JObject)jo["fieldsListStr"];
            fieldDetail = fieldsList;
            fieldA = (JObject)jo["fieldAStr"];
            fieldE = (JObject)jo["fieldEStr"];
        }
    }
}
