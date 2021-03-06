﻿using AccountApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Action.StudentAccount
{
    class AddToDirectoryAndSmartschool : AccountAction
    {
        public AddToDirectoryAndSmartschool() : base(
            "Maak een Nieuw Account",
            "Voeg een account toe aan Active Directory en Smartschool.",
            true, true)
        {

        }

        public async override Task Apply(State.Linked.LinkedAccount linkedAccount, DateTime deletionDate)
        {
            var wisa = linkedAccount.Wisa.Account;
            var directory = await AccountApi.Directory.AccountManager.Create(wisa.FirstName, wisa.Name, wisa.WisaID, AccountRole.Student, wisa.ClassGroup).ConfigureAwait(false);

            if (directory != null)
            {
                MainWindow.Instance.Log.AddMessage(Origin.Directory, "Added account for " + wisa.FullName);
                await AddToSmartschool.Add(linkedAccount, wisa, directory).ConfigureAwait(false);
            }
            else
            {
                MainWindow.Instance.Log.AddError(Origin.Directory, "Failed to add " + wisa.FullName);
            }
        }

        public static void Evaluate(State.Linked.LinkedAccount account)
        {
            if (account.Wisa.Exists && !account.Directory.Exists && !account.Smartschool.Exists)
            {
                account.Actions.Add(new AddToDirectoryAndSmartschool());
            }
        }
    }
}
