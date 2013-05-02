using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Windows.Input;

namespace PairPicker
{
    public class PairPickerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private readonly ICollection<string> _users;
        private readonly string _path;
        private readonly ICollection<string> _selectedUsers;
        private readonly FileManager _fileManager;

        public PairPickerViewModel()
        {
            _users = new ObservableCollection<string>(ConfigurationManager.AppSettings["users"].Split(',').Select(u => u.Trim()));
            _path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\.gitconfig";
            _selectedUsers = new Collection<string>();
            _fileManager = new FileManager();
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
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var userList = GenerateUserList();
            configFile.AppSettings.Settings["users"].Value = userList;

            configFile.Save();
        }

        private string GenerateUserList()
        {
            if (_users.Count == 0)
                return string.Empty;
            var firstUser = _users.First();
            var remainingUsers = _users.Except(new List<string> {firstUser});

            return remainingUsers.Aggregate(firstUser, (acc, current) => acc += ", " + current);
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
            var inLines = _fileManager.LoadFile(_path);
            var outLines = new List<string>();
            foreach (var line in inLines)
            {
                if (line.TrimStart().StartsWith("name = "))
                {
                    var userString = _selectedUsers.DefaultIfEmpty("msms")
                        .Aggregate((acc, entry) => string.IsNullOrEmpty(acc) ? entry : acc + ", " + entry);
                    outLines.Add(string.Format("\tname = {0}", userString));
                }
                else
                {
                    outLines.Add(line);
                }
            }
            _fileManager.WriteFile(_path, outLines.ToArray());
        }


    }
}
