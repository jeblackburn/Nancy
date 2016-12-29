namespace Nancy.Demo.Authentication.Forms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;

    using Nancy.Authentication.Forms;

    public class UserDatabase : IUserMapper
    {
        private static List<Tuple<string, string, Guid>> users = new List<Tuple<string, string, Guid>>();

        static UserDatabase()
        {
            users.Add(new Tuple<string, string, Guid>("admin", "password", new Guid("55E1E49E-B7E8-4EEA-8459-7A906AC4D4C0")));
            users.Add(new Tuple<string, string, Guid>("user", "password", new Guid("56E1E49E-B7E8-4EEA-8459-7A906AC4D4C0")));
        }

        public ClaimsPrincipal GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            try
            {
                var userRecord = users.FirstOrDefault(u => u.Item3 == identifier);

                if (userRecord == null)
                    return null;

                var claims = new List<Claim> { new Claim(ClaimTypes.Name, userRecord.Item1) };
                if (userRecord.Item1 == "admin")
                {
                    claims.Add(new Claim(ClaimTypes.GroupSid, "administrators"));
                }
                var cid = new ClaimsIdentity(claims, "password", ClaimTypes.Name, null);
                return new ClaimsPrincipal(cid);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public static Guid? ValidateUser(string username, string password)
        {
            var userRecord = users.FirstOrDefault(u => u.Item1 == username && u.Item2 == password);

            if (userRecord == null)
            {
                return null;
            }

            return userRecord.Item3;
        }
    }
}