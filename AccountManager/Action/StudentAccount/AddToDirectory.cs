﻿using AccountApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Action.StudentAccount
{
    class AddToDirectory : AccountAction
    {
        public AddToDirectory() : base(
            "Maak een Nieuw Directory Account",
            "Voeg een account toe aan Active Directory.",
            true, true)
        {

        } 

        public async override Task Apply(State.Linked.LinkedAccount linkedAccount, DateTime deletionDate)
        {
            var wisa = linkedAccount.Wisa.Account;
            var smartschool = linkedAccount.Smartschool.Account;

            var usernameExists = await AccountApi.Directory.AccountManager.Exists(smartschool.UID).ConfigureAwait(false);

            var directory = await AccountApi.Directory.AccountManager.Create(
                wisa.FirstName, 
                wisa.Name, 
                wisa.WisaID, 
                AccountRole.Student,
                wisa.ClassGroup,
                usernameExists ? "": smartschool.UID
            ).ConfigureAwait(false);

            if (directory != null)
            {
                MainWindow.Instance.Log.AddMessage(Origin.Directory, "Added account for " + wisa.FullName);
            }
            else
            {
                MainWindow.Instance.Log.AddError(Origin.Directory, "Failed to add " + wisa.FullName);
            }
        }

        public static void Evaluate(State.Linked.LinkedAccount account)
        {
            if (account.Wisa.Exists && !account.Directory.Exists && account.Smartschool.Exists)
            {
                account.Actions.Add(new AddToDirectory());
            }
        }
    }
}
