using System.Reflection.PortableExecutable;


namespace adcon
{
    public class User
    {
        public String? samAccountName { get; set; }
        public String? principalName { get; set; }
        public String? displayName { get; set; }
        public String? givenName { get; set; }
        public String? sn { get; set; }
        public String? initials { get; set; }
        public String? description { get; set; }
        public String? title { get; set; }
        public String? department { get; set; }
        public String? company { get; set; }
        public String? mail { get; set; }
        public String? telephoneNumber { get; set; }
        public String? mobile { get; set; }
        public String? streetAddress { get; set; }
        public String? city { get; set; }
        public String? state { get; set; }
        public String? postalCode { get; set; }
        public String? country { get; set; }
        public String? memberOf { get; set; }
    }
    public class ConnectionInfo
    {
        public String URI { get; set; }
        public String adminName { get; set; }
        public String adminPass { get; set; }
        public String baseDn { get; set; }
    }
    public class ConnectionInfoWithUser : User
    {
        public String URI { get; set; }
        public String adminName { get; set; }
        public String adminPass { get; set; }
        public String baseDn { get; set; }
    }
    public class AddUser : ConnectionInfoWithUser
    {
        public String newUserName { get; set; }
        public String newUserPass { get; set; }
        public int? userAccountControl { get; set; } = 0x0200;
    }
    public class ChangeUserAtttributes : ConnectionInfoWithUser
    {
        public String userName { get; set; }
        public String? userPass { get; set; }
        public String? userNewPass { get; set; }
        public int? userAccountControl { get; set; }
    }
    public class DeleteUser : ConnectionInfo
    {
        public String userName { get; set; }
    }
}
