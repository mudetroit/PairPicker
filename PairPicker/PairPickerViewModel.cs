using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;

namespace PairPicker
{
    public class PairPickerViewModel
    {
        private readonly IEnumerable<string> _users;
        private readonly string _path;
        private readonly ICollection<string> _selectedUsers;
        private readonly FileManager _fileManager;

        public PairPickerViewModel()
        {
            _users = ConfigurationManager.AppSettings["users"].Split(',').Select(u => u.Trim()).ToList();
            _path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\.gitconfig";
            _selectedUsers = new Collection<string>();
            _fileManager = new FileManager();
        }

        public IEnumerable<dynamic> Users
        {
            get { return _users.Select(u => new {Name = u, Command = new RelayCommand(ToggleUser)}); }
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
