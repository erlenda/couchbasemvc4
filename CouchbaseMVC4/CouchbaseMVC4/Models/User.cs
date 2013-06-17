using System;

namespace CouchbaseMVC4.Models

{
    [Serializable]
    public class User
    {
        public Guid UserId { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public String FullName { get; set; }
    }
}