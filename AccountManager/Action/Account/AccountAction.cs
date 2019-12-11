﻿using System;
using System.ComponentModel;
using System.Threading.Tasks;
using static AbstractAccountApi.ObservableProperties;

namespace AccountManager.Action.Account
{

    public abstract class AccountAction : INotifyPropertyChanged
    {
        private string header;
        public string Header => header;

        private string description;
        public string Description => description;

        private bool canBeApplied;
        public bool CanBeApplied => canBeApplied;

        private bool canBeAppliedToAll = false;
        public bool CanBeAppliedToAll => canBeAppliedToAll;

        public Prop<bool> ApplyToAll { get; set; } = new Prop<bool>() { Value = false };

        bool indicator = false;
        public bool Indicator
        {
            get => indicator;
            set
            {
                indicator = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Indicator)));
            }
        }

        public abstract Task Apply(State.Linked.LinkedAccount linkedAccount, DateTime deletionDate);

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public AccountAction(string header, string description, bool canBeApplied, bool canBeAppliedToAll = false)
        {
            this.header = header;
            this.description = description;
            this.canBeApplied = canBeApplied;
            this.canBeAppliedToAll = canBeAppliedToAll;
        }
    }
}
