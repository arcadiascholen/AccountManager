﻿using AccountApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Views.Dialogs
{
    interface IRuleEditor
    {
        IRule Rule { get; set; }
    }
}
