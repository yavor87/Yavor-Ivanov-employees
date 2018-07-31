using Employees.Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Employees.UI.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.LoadFileCommand = new DelegateCommand(LoadFile);
        }

        private IReadOnlyCollection<EmployeeRecord> records;
        private IEnumerable<ProjectEmployeeMatch> matches;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand LoadFileCommand { get; }

        public IEnumerable<ProjectEmployeeMatch> Matches
        {
            get => this.matches;
            private set
            {
                if (this.matches == value)
                    return;

                this.matches = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Matches)));
            }
        }

        private async void LoadFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() != true)
            {
                return;
            }

            try
            {
                using (Stream fs = ofd.OpenFile())
                {
                    EmployeeReader reader = new EmployeeReader();
                    this.records = await reader.ReadRecordsAsync(fs);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error reading file");
                return;
            }

            ProjectEmployeeMatcher matcher = new ProjectEmployeeMatcher();
            this.Matches = matcher.FindMatches(this.records);
        }
    }
}
