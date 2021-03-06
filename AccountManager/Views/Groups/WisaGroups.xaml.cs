﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace AccountManager.Views.Groups
{
    enum FilterType
    {
        Name,
        Description,
    }
    /// <summary>
    /// Interaction logic for WisaGroups.xaml
    /// </summary>
    /// 
    public partial class WisaGroups : UserControl
    {
        public Prop<bool> ShowGroupsReloadButtonIndicator { get; set; } = new Prop<bool> { Value = false };
        public Prop<string> GroupCount { get; set; } = new Prop<string> { Value = "0" };
        public State.App AppState { get => State.App.Instance; }

        private ObservableCollection<AccountApi.Wisa.ClassGroup> groups = new ObservableCollection<AccountApi.Wisa.ClassGroup>();

        private FilterType FilterType { get; set; } = FilterType.Name;
        private string Filter { get; set; } = String.Empty;

        public WisaGroups()
        {
            InitializeComponent();
            MainGrid.DataContext = this;
            CreateSelection();
            GroupList.ItemsSource = groups;
            
        }

        private void CreateSelection()
        {
            groups.Clear();
            foreach(var group in AccountApi.Wisa.ClassGroupManager.All)
            {
                if(Filter == string.Empty)
                {
                    groups.Add(group);
                } else if(FilterType == FilterType.Name)
                {
                    if (group.Name.Contains(Filter)) groups.Add(group);
                } else if (FilterType == FilterType.Description)
                {
                    if (group.Description.Contains(Filter)) groups.Add(group);
                }
            }
            GroupCount.Value = groups.Count.ToString();
        }

        private async void ReloadGroupsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowGroupsReloadButtonIndicator.Value = true;
            AppState.Wisa.Connect();
            await AppState.Wisa.Groups.Load();
            await State.App.Instance.Linked.Groups.ReLink();
            CreateSelection();
            ShowGroupsReloadButtonIndicator.Value = false;
        }

        private void FilterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = (sender as ComboBox).SelectedIndex;
            if (index == 0) FilterType = FilterType.Name;
            else FilterType = FilterType.Description;
            CreateSelection();
        }

        private void FilterText_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter = (sender as TextBox).Text.Trim();
            CreateSelection();
        }

        private void FilterDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Filter = string.Empty;
            FilterText.Text = string.Empty;
            CreateSelection();
        }
    }
}
