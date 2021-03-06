﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.State.AD
{
    public class ADAccounts
    {
        DateTime lastSync;
        public DateTime LastSync => lastSync;

        const string fileName = "directoryAccounts.json";

        public async Task Load()
        {
            await AccountApi.Directory.AccountManager.LoadStaff().ConfigureAwait(false);
            await AccountApi.Directory.AccountManager.LoadStudents().ConfigureAwait(false);

            AccountApi.Directory.AccountManager.ApplyImportRules(App.Instance.AD.ImportRules.ToList());

            lastSync = DateTime.Now;
            SaveToJson();
            App.Instance.AD.UpdateObservers();
        }

        public void LoadFromJson()
        {
            var location = Path.Combine(App.GetAppFolder(), fileName);
            if (File.Exists(location))
            {
                string content = File.ReadAllText(location);
                var newObj = JObject.Parse(content);
                AccountApi.Directory.AccountManager.FromJson(newObj);
                lastSync = newObj.ContainsKey("lastSync") ? Convert.ToDateTime(newObj["lastSync"]) : DateTime.MinValue;
                App.Instance.AD.UpdateObservers();
            }
        }

        public void SaveToJson()
        {
            var json = AccountApi.Directory.AccountManager.ToJson();
            json["lastSync"] = lastSync;
            var location = Path.Combine(App.GetAppFolder(), fileName);
            File.WriteAllText(location, json.ToString());
        }
    }
}
