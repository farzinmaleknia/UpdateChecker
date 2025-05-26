
using System.Resources;
using System.Collections;
using System.Xml;

namespace Api.Services.Resources;

public class ResourcesService
{
    public async Task<ResultClass<Dictionary<string, Dictionary<string, string>>>> GetResources()
    {    
        var result = new ResultClass<Dictionary<string, Dictionary<string, string>>>(); 

        try
        {
            var groupedTranslations = new Dictionary<string, Dictionary<string, string>>();

            var directory = Directory.GetCurrentDirectory();
            var resPath = Path.Combine(directory, @"Resources\");
            var resxFiles = Directory.GetFiles(resPath, "*.resx", SearchOption.TopDirectoryOnly);

            foreach (var file in resxFiles)
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(file);

                string groupName = Path.GetFileNameWithoutExtension(file);
                if (groupName == "Defaults")
                {
                    continue;
                }

                var entries = new Dictionary<string, string>();


                var nodes = xmlDoc.SelectNodes("/root/data");
                if (nodes != null)
                {
                    foreach (XmlNode node in nodes)
                    {
                        var name = node.Attributes?["name"]?.Value;
                        var value = node.SelectSingleNode("value")?.InnerText;

                        if (!string.IsNullOrEmpty(name) && value != null)
                        {
                            entries[name] = value;
                        }
                    }
                }

                groupedTranslations[groupName] = entries;
            }

            //var resourcesJson = Newtonsoft.Json.JsonConvert.SerializeObject(groupedTranslations);

            result.Data = groupedTranslations;
            result.IsSuccess = true;
            return result;

        }
        catch (Exception e)
        {
            result.MessageKey = e.Message;
            return result;
        }   
    }
}
