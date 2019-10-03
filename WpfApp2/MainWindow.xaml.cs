﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WpfApp2 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
        }

        //   Variables section

        public string settingsFilePath = @"C:\temp\sample.txt";
        // TODO: calculate proper value above
        public bool isModified = false;

        // internal functions

        private bool DiscardChanges() {
            string message = "Settings content was modified. Are you sure you want to abondon changes?";
            var response = MessageBox.Show(message, "Settings modified", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return response == MessageBoxResult.Yes;
        }

        private bool ValidateJSON(string str) {
            try {
                var obj = JsonSerializer.Deserialize<Object>(str);
                return true;
            } catch {
                return false;
            }
        }

        // GUI events handling

        private void LoadButton_Click(object sender, RoutedEventArgs e) {
            if (isModified && DiscardChanges()) {
                isModified = false;
            }

            if (!isModified) {
                TextBox.Text = System.IO.File.ReadAllText(settingsFilePath);
                isModified = false;

                TextBox.IsEnabled = true;
                LoadButton.IsEnabled = false;
                SaveButton.IsEnabled = false;
                // update status
            }

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
            LoadButton.IsEnabled = true;
            SaveButton.IsEnabled = true;
            isModified = true;
            // update status
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e) {
            if (!ValidateJSON(TextBox.Text)) {
                MessageBox.Show("Not a valid JSON");
                return;
            }

            File.WriteAllText(settingsFilePath, TextBox.Text);
            LoadButton.IsEnabled = false;
            SaveButton.IsEnabled = false;
            isModified = false;
            // set status
            MessageBox.Show("File saved!");

        }

        private void ExitButton_Click(object sender, RoutedEventArgs e) {
            if (!isModified || DiscardChanges()) {
                Application.Current.Shutdown();
            }

        }
    }
}
