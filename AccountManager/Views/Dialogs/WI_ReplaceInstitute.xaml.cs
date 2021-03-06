﻿using AbstractAccountApi;
using AccountApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static AbstractAccountApi.ObservableProperties;

namespace AccountManager.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for WI_ReplaceInstitute.xaml
    /// </summary>
    public partial class WI_ReplaceInstitute : UserControl, IRuleEditor
    {
        public IRule Rule { get; set; }
        public Prop<string> Original { get; set; } = new Prop<string> { Value = "" };
        public Prop<string> Replacement { get; set; } = new Prop<string> { Value = "" };

        public WI_ReplaceInstitute(IRule rule)
        {
            InitializeComponent();
            Rule = rule;
            DataContext = this;
            Original.Value = rule.GetConfig(0);
            Replacement.Value = rule.GetConfig(1);
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            Rule.SetConfig(0, Original.Value.Trim());
            Rule.SetConfig(1, Replacement.Value.Trim());
        }
    }
}
