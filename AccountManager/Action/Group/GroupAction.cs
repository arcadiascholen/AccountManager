﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using static AbstractAccountApi.ObservableProperties;

namespace AccountManager.Action.Group
{
    public abstract class GroupAction
    {
        private string header;
        public string Header => header;

        private string description;
        public string Description => description;

        private bool canBeApplied;
        public bool CanBeApplied => canBeApplied;
        public bool CanShowDetails { get; protected set; } = false;

        public Prop<bool> InProgress { get; set; } = new Prop<bool>() { Value = false };

        public virtual FlowDocument GetDetails(State.Linked.LinkedGroup group) { return null; }

        public abstract Task Apply(State.Linked.LinkedGroup linkedGroup);

        public GroupAction(string header, string description, bool canBeApplied)
        {
            this.header = header;
            this.description = description;
            this.canBeApplied = canBeApplied;
        }
    }
}
