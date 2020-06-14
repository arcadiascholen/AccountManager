﻿using AccountApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Action.StudentAccount
{
    class AddToSmartschool : AccountAction
    {
        public AddToSmartschool() : base(
            "Maak een Nieuw Smartschool Account",
            "Voeg een account toe aan Smartschool",
            true)
        {

        }

        public async override Task Apply(State.Linked.LinkedAccount linkedAccount, DateTime deletionDate)
        {
            var wisa = linkedAccount.Wisa.Account;
            var directory = linkedAccount.Directory.Account;

            await Add(linkedAccount, wisa, directory).ConfigureAwait(false);
        }

        public static async Task Add(State.Linked.LinkedAccount linkedAccount, AccountApi.Wisa.Student wisa, AccountApi.Directory.Account directory)
        {
            var ssAccount = new AccountApi.Smartschool.Account();

            ssAccount.UID = directory.UID;
            ssAccount.RegisterID = wisa.StateID;
            try
            {
                ssAccount.StemID = Convert.ToInt32(wisa.StemID);
            }
            catch (Exception)
            {
                ssAccount.StemID = 0;
            }

            ssAccount.Role = AccountRole.Student;
            ssAccount.GivenName = wisa.FirstName;
            ssAccount.SurName = wisa.Name;
            ssAccount.Gender = wisa.Gender;
            ssAccount.Birthday = wisa.DateOfBirth;
            ssAccount.BirthPlace = wisa.PlaceOfBirth;
            ssAccount.Street = wisa.Street;
            ssAccount.HouseNumber = wisa.HouseNumber;
            ssAccount.HouseNumberAdd = wisa.HouseNumberAdd;
            ssAccount.PostalCode = wisa.PostalCode;
            ssAccount.City = wisa.City;
            ssAccount.Mail = directory.UID + "@" + AccountApi.Directory.Connector.AzureDomain;

            var result = await AccountApi.Smartschool.AccountManager.Save(ssAccount, "FakeP4ssword").ConfigureAwait(false);
            if (!result)
            {
                MainWindow.Instance.Log.AddError(Origin.Smartschool, "Failed to add " + wisa.FullName);
                return;
            }
            else
            {

                linkedAccount.Smartschool.Account = ssAccount;
                MainWindow.Instance.Log.AddMessage(Origin.Smartschool, "Added account for " + wisa.FullName);
            }


            IGroup classgroup;
            if (wisa.ClassGroup.Contains("ANS") || wisa.ClassGroup.Contains("BNS"))
            {
                classgroup = AccountApi.Smartschool.GroupManager.Root.Find("Leerlingen");
            }
            else
            {
                classgroup = AccountApi.Smartschool.GroupManager.Root.Find(wisa.ClassGroup);
            }

            await MoveToSmartschoolClassGroup.Move(linkedAccount, classgroup).ConfigureAwait(false);
        }

        public static void Evaluate(State.Linked.LinkedAccount account)
        {
            if (account.Wisa.Exists && account.Directory.Exists && !account.Smartschool.Exists)
            {
                account.Actions.Add(new AddToSmartschool());
            }
        }
    }
}
