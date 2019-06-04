﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Action
{
    public class NoAccountActionNeeded : AccountAction
    {
        public NoAccountActionNeeded() : base(
            "Geen Actie Nodig", "Dit account voldoet aan alle regels.", false)
        {
        }

        public override Task Apply(LinkedAccount linkedAccount)
        {
            throw new NotImplementedException();
        }
    }
}