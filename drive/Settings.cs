using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace KosmoConsole
{
    public class Settings
    {
        public class UserEntry
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        public List<UserEntry> Users;
        public string[] UserValues;

        public void LoadUserData()
        {
            Users = new List<UserEntry>();
            UserValues = Array.Empty<string>();
            try
            {
                string json = File.ReadAllText("uinfo.json");
                var root = JObject.Parse(json);
                var userData = root.SelectToken("sys.users.u1");
                if (userData is JObject stringsObj)
                {
                    var valuesList = new List<string>();
                    foreach (var prop in stringsObj.Properties())
                    {
                        string key = prop.Name;
                        string value = prop.Value?.ToString() ?? string.Empty;
                        Users.Add(new UserEntry { Key = key, Value = value });
                        valuesList.Add(value);
                    }
                    UserValues = valuesList.ToArray();
                }
            }
            catch (Exception ex)
            {
                var a = new Kernel();
                a.InitFail("users data", ex.Message.ToString());
            }
        }

        public string GetUserInfo(string key)
        {
            if (Users != null)
            {
                var entry = Users.Find(u => u.Key == key);
                if (entry != null)
                    return entry.Value;
            }
            return string.Empty;
        }

        public void PwdChange(string newPass)
        {
            if (Users != null)
            {
                var entry = Users.Find(u => u.Key == "password");
                if (entry != null)
                {
                    entry.Value = newPass;
                    //SaveUserData();
                }
            }
        }
    }
}
