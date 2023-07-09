using Microsoft.AspNetCore.Mvc;
using System.DirectoryServices;
using System.Text.Json;

namespace adcon.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OuController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetOu(String URI, String baseDn)
        {
            var response = new List<Dictionary<string, string>>();
            try
            {
                DirectoryEntry directoryEntry = new DirectoryEntry($"{URI}/{baseDn}");
                DirectorySearcher searcher = new DirectorySearcher(directoryEntry);
                searcher.Filter = "(objectCategory=organizationalUnit)";

                SearchResultCollection results = searcher.FindAll();

                foreach (SearchResult result in results)
                {
                    DirectoryEntry ouEntry = result.GetDirectoryEntry();
                    string ouName = ouEntry.Properties["name"].Value.ToString();
                    string distinguishedname = ouEntry.Properties["distinguishedname"].Value.ToString();
                    response.Add(new Dictionary<string, string>() { { "name", ouName }, { "distinguishedname", distinguishedname } });
                    ouEntry.Close();
                }

                string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                directoryEntry.Close();

                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult AddOu([FromBody] Ou o)
        {
            try
            {
                DirectoryEntry directoryEntry = new DirectoryEntry($"{o.URI}/{o.baseDn}", o.adminName, o.adminPass);
                DirectoryEntry newOU = directoryEntry.Children.Add("OU=" + o.ouName, "organizationalUnit");
                newOU.CommitChanges();

                string jsonData = JsonSerializer.Serialize(o);

                newOU.Close();
                directoryEntry.Close();

                return CreatedAtAction("GetOu", jsonData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        public IActionResult DeleteOu([FromBody] Ou o)
        {
            try
            {
                DirectoryEntry directoryEntry = new DirectoryEntry($"{o.URI}/{o.baseDn}", o.adminName, o.adminPass);
                DirectoryEntry ouEntry = directoryEntry.Children.Find($"OU={o.ouName}", "organizationalUnit");
                
                ouEntry.DeleteTree();
                ouEntry.Close();
                directoryEntry.Close();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
