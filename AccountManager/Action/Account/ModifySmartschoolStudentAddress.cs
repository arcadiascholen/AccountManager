﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Action.Account
{
    class ModifySmartschoolStudentAddress : AccountAction
    {
        public ModifySmartschoolStudentAddress() : base(
            "Wijzig adres in smartschool",
            "Het adres in Wisa is verschillend van dat in smartschool", true)
        {

        }

        public async override Task Apply(State.Linked.LinkedAccount linkedAccount, DateTime deletionDate)
        {
            linkedAccount.Smartschool.Account.City = linkedAccount.Wisa.Account.City;
            linkedAccount.Smartschool.Account.Street = linkedAccount.Wisa.Account.Street;
            linkedAccount.Smartschool.Account.PostalCode = linkedAccount.Wisa.Account.PostalCode;
            linkedAccount.Smartschool.Account.HouseNumber = linkedAccount.Wisa.Account.HouseNumber;
            linkedAccount.Smartschool.Account.HouseNumberAdd = linkedAccount.Wisa.Account.HouseNumberAdd;

            await AccountApi.Smartschool.AccountManager.Save(linkedAccount.Smartschool.Account, "").ConfigureAwait(false);
        }

        public static void Evaluate(State.Linked.LinkedAccount account)
        {
            if (!checkUserHomeAddress(account))
            {
                account.Smartschool.FlagWarning();
                account.Actions.Add(new ModifySmartschoolStudentAddress());
                account.OK = false;
            }
        }

        private static bool checkUserHomeAddress(State.Linked.LinkedAccount account)
        {
            if (account.Wisa.Account.City != account.Smartschool.Account.City) return false;
            if (account.Wisa.Account.Street != account.Smartschool.Account.Street) return false;
            if (account.Wisa.Account.PostalCode != account.Smartschool.Account.PostalCode) return false;
            if (account.Wisa.Account.HouseNumber != account.Smartschool.Account.HouseNumber) return false;
            if (account.Wisa.Account.HouseNumberAdd != account.Smartschool.Account.HouseNumberAdd) return false;
            return true;
        }
    }
}
