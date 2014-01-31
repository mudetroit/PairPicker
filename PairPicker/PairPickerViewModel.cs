using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using PairPicker.Properties;

namespace PairPicker
{
    public class PairPickerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private readonly ICollection<string> _users;
        private readonly ICollection<string> _selectedUsers;

        public PairPickerViewModel()
        {
            _users = new ObservableCollection<string>(Settings.Default.Users);
            _selectedUsers = new Collection<string>();
            AddUserCommand = new RelayCommand(AddUser);
        }

        public ICommand AddUserCommand { get; private set; }

        public IEnumerable<dynamic> Users
        {
            get { return _users.Select(u => new {Name = u, Command = new RelayCommand(ToggleUser)}); }
        }

        private void AddUser(object nameObject)
        {
            var newUser = Convert.ToString(nameObject);
            if (!_users.Contains(newUser) && !string.IsNullOrWhiteSpace(newUser))
            {
                _users.Add(newUser);
                PropertyChanged(this, new PropertyChangedEventArgs("Users"));
                UpdateConfig();
            }
        }

        private void UpdateConfig()
        {
            Settings.Default.Users = _users;
            Settings.Default.Save();
        }

        private void ToggleUser(object nameObject)
        {
            var name = nameObject as string;
            if (name != null)
            {
                if (_selectedUsers.Contains(name))
                    _selectedUsers.Remove(name);
                else
                    _selectedUsers.Add(name);

                UpdateGitInfo();
            }

        }

        private void UpdateGitInfo()
        {
            var userString = _selectedUsers.DefaultIfEmpty("msms").Aggregate((acc, entry) => string.IsNullOrEmpty(acc) ? entry : acc + ", " + entry);
            var arguments = "config --global user.name \"" + userString + "\"";
            var processInfo = new ProcessStartInfo("git", arguments)
            {
                UseShellExecute = false,
                RedirectStandardError = true,
                CreateNoWindow = true, 
            };
            Process.Start(processInfo);
        }
    }
}
