using CaloriePlannerApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaloriePlannerApp.Data.Services
{
    public class UserService
    {
        private Entities.User _currentUser;
        private DataContext _context;
        public Action<string> userNameChanged;
        public UserService(DataContext dataContext)
        {
            _context = dataContext;
            var storedId = Preferences.Get("CurrentUserId", null);
            var idFound = Guid.TryParse(storedId, out var actualId);
            if (idFound && storedId != null)
            {
                var user = _context.Users.FirstOrDefault(x => x.Id == actualId);
                if(user != null)
                {
                    _currentUser = user;
                }
            }
        }
        public Entities.User CurrentUser
        {
            get => _currentUser;
            set 
            { 
                _currentUser = value;
                Preferences.Set("CurrentUserId", _currentUser?.Id.ToString());
                userNameChanged.Invoke(Name);
            }
        }
        public string Name
        {
            get => _currentUser?.UserFullName() ?? "Select a user";
        }

        public void ClearUser()
        {
            _currentUser = null;
        }
    }
}
