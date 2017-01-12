﻿using System.Windows;

namespace EngineCell.Application.Views.Welcome
{
    /// <summary>
    /// Interaction logic for NewTestDialog.xaml
    /// </summary>
    public partial class NewTestDialog : Window
    {
        public string NewTestName { get; set; }
        public string NewTestDescription { get; set; }

        public NewTestDialog()
        {
            InitializeComponent();
            Name.Focus();
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            ErrorMessage.Content = "";
            if (string.IsNullOrEmpty(Name.Text))
            {
                ErrorMessage.Content = "Test Name is required.";
                return;
            }
            NewTestName = Name.Text;
            NewTestDescription = Description.Text;
            Close();
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            NewTestName = "";
            NewTestDescription = "";
            Close();
        }
    }
}
