using Microsoft.AspNetCore.Mvc;
using System.DirectoryServices;
using System.Text.Json;

namespace adcon.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetUser(String userName, String URI, String baseDn)
        {
            try
            {
                DirectoryEntry directoryEntry = new DirectoryEntry($"{URI}/{baseDn}");
                string filter = $"(&(objectCategory=User)(sAMAccountName={userName}))";
                var searcher = new DirectorySearcher(directoryEntry, filter);
                SearchResult result = searcher.FindOne();

                if (result != null)
                {
                    var user = result.GetDirectoryEntry();
                    User userAttributes = new User
                    {
                        samAccountName = user.Properties["samAccountName"].Value != null ? user.Properties["samAccountName"].Value.ToString() : "undefined",
                        principalName = user.Properties["principalName"].Value != null ? user.Properties["principalName"].Value.ToString() : "undefined",
                        displayName = user.Properties["displayName"].Value != null ? user.Properties["displayName"].Value.ToString() : "undefined",
                        givenName = user.Properties["givenName"].Value != null ? user.Properties["givenName"].Value.ToString() : "undefined",
                        sn = user.Properties["sn"].Value != null ? user.Properties["sn"].Value.ToString() : "undefined",
                        initials = user.Properties["initials"].Value != null ? user.Properties["initials"].Value.ToString() : "undefined",
                        description = user.Properties["description"].Value != null ? user.Properties["description"].Value.ToString() : "undefined",
                        title = user.Properties["title"].Value != null ? user.Properties["title"].Value.ToString() : "undefined",
                        department = user.Properties["department"].Value != null ? user.Properties["department"].Value.ToString() : "undefined",
                        company = user.Properties["company"].Value != null ? user.Properties["company"].Value.ToString() : "undefined",
                        mail = user.Properties["mail"].Value != null ? user.Properties["mail"].Value.ToString() : "undefined",
                        telephoneNumber = user.Properties["telephoneNumber"].Value != null ? user.Properties["telephoneNumber"].Value.ToString() : "undefined",
                        mobile = user.Properties["mobile"].Value != null ? user.Properties["mobile"].Value.ToString() : "undefined",
                        streetAddress = user.Properties["streetAddress"].Value != null ? user.Properties["streetAddress"].Value.ToString() : "undefined",
                        city = user.Properties["city"].Value != null ? user.Properties["city"].Value.ToString() : "undefined",
                        state = user.Properties["state"].Value != null ? user.Properties["state"].Value.ToString() : "undefined",
                        postalCode = user.Properties["postalCode"].Value != null ? user.Properties["postalCode"].Value.ToString() : "undefined",
                        country = user.Properties["country"].Value != null ? user.Properties["country"].Value.ToString() : "undefined",
                        memberOf = user.Properties["memberOf"].Value != null ? user.Properties["memberOf"].Value.ToString() : "undefined"
                    };
                    
                    string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(userAttributes);

                    directoryEntry.Close();

                    return Ok(jsonData);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult AddUser([FromBody] AddUser a)
        {
            DirectoryEntry directoryEntry = new DirectoryEntry($"{a.URI}/{a.baseDn}", a.adminName, a.adminPass);

            try
            {
                DirectoryEntry childEntry = directoryEntry.Children.Add($"CN={a.newUserName}", "user");

                if (a.samAccountName != null)
                    childEntry.Properties["samAccountName"].Value = a.samAccountName;
                if (a.principalName != null)
                    childEntry.Properties["principalName"].Value = a.principalName;
                if (a.displayName != null)
                    childEntry.Properties["displayName"].Value = a.displayName;
                if (a.givenName != null)
                    childEntry.Properties["givenName"].Value = a.givenName;
                if (a.sn != null)
                    childEntry.Properties["sn"].Value = a.sn;
                if (a.initials != null)
                    childEntry.Properties["initials"].Value = a.initials;
                if (a.description != null)
                    childEntry.Properties["description"].Value = a.description;
                if (a.title != null)
                    childEntry.Properties["title"].Value = a.title;
                if (a.department != null)
                    childEntry.Properties["department"].Value = a.department;
                if (a.company != null)
                    childEntry.Properties["company"].Value = a.company;
                if (a.mail != null)
                    childEntry.Properties["mail"].Value = a.mail;
                if (a.telephoneNumber != null)
                    childEntry.Properties["telephoneNumber"].Value = a.telephoneNumber;
                if (a.mobile != null)
                    childEntry.Properties["mobile"].Value = a.mobile;
                if (a.streetAddress != null)
                    childEntry.Properties["streetAddress"].Value = a.streetAddress;
                if (a.city != null)
                    childEntry.Properties["city"].Value = a.city;
                if (a.state != null)
                    childEntry.Properties["state"].Value = a.state;
                if (a.postalCode != null)
                    childEntry.Properties["postalCode"].Value = a.postalCode;
                if (a.country != null)
                    childEntry.Properties["country"].Value = a.country;
                if (a.memberOf != null)
                    childEntry.Properties["memberOf"].Value = a.memberOf;

                childEntry.CommitChanges();
                directoryEntry.CommitChanges();

                childEntry.Invoke("SetPassword", new object[] { a.newUserPass });
                childEntry.Properties["userAccountControl"].Value = a.userAccountControl;

                childEntry.CommitChanges();
                
                string jsonData = JsonSerializer.Serialize(a);

                childEntry.Close();
                directoryEntry.Close();

                return CreatedAtAction("PostUser", jsonData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult ChangeUserAttributes([FromBody] ChangeUserAtttributes c)
        {
            DirectoryEntry directoryEntry = new DirectoryEntry($"{c.URI}/{c.baseDn}", c.adminName, c.adminPass);

            try
            {
                DirectoryEntry childEntry = directoryEntry.Children.Find($"CN={c.userName}", "user");

                if (c.samAccountName != null)
                    childEntry.Properties["samAccountName"].Value = c.samAccountName;
                if (c.principalName != null)
                    childEntry.Properties["principalName"].Value = c.principalName;
                if (c.displayName != null)
                    childEntry.Properties["displayName"].Value = c.displayName;
                if (c.givenName != null)
                    childEntry.Properties["givenName"].Value = c.givenName;
                if (c.sn != null)
                    childEntry.Properties["sn"].Value = c.sn;
                if (c.initials != null)
                    childEntry.Properties["initials"].Value = c.initials;
                if (c.description != null)
                    childEntry.Properties["description"].Value = c.description;
                if (c.title != null)
                    childEntry.Properties["title"].Value = c.title;
                if (c.department != null)
                    childEntry.Properties["department"].Value = c.department;
                if (c.company != null)
                    childEntry.Properties["company"].Value = c.company;
                if (c.mail != null)
                    childEntry.Properties["mail"].Value = c.mail;
                if (c.telephoneNumber != null)
                    childEntry.Properties["telephoneNumber"].Value = c.telephoneNumber;
                if (c.mobile != null)
                    childEntry.Properties["mobile"].Value = c.mobile;
                if (c.streetAddress != null)
                    childEntry.Properties["streetAddress"].Value = c.streetAddress;
                if (c.city != null)
                    childEntry.Properties["city"].Value = c.city;
                if (c.state != null)
                    childEntry.Properties["state"].Value = c.state;
                if (c.postalCode != null)
                    childEntry.Properties["postalCode"].Value = c.postalCode;
                if (c.country != null)
                    childEntry.Properties["country"].Value = c.country;
                if (c.memberOf != null)
                    childEntry.Properties["memberOf"].Value = c.memberOf;
                if (c.userAccountControl != null)
                    childEntry.Properties["userAccountControl"].Value = c.userAccountControl;

                if (c.userPass != null && c.userNewPass != null)
                    childEntry.Invoke("ChangePassword", c.userPass, c.userNewPass);

                childEntry.CommitChanges();

                childEntry.Close();
                directoryEntry.Close();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public IActionResult DeleteUser([FromBody] DeleteUser u)
        {
            DirectoryEntry directoryEntry = new DirectoryEntry($"{u.URI}/{u.baseDn}", u.adminName, u.adminPass);

            try
            {
                DirectoryEntry userEntry = directoryEntry.Children.Find($"CN={u.userName}", "user");
                userEntry.DeleteTree();

                userEntry.Close();
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
