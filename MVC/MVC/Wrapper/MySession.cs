using MVC.Models;
using MVC.Extensions;
using Microsoft.AspNetCore.Http;

namespace MVC.Wrapper 
{
    public class MySession 
    {
        private const string UserKey = "user";  

        private ISession _session;

        public MySession(ISession session)
        {
            _session = session;
        }

        /// <summary>
        /// Gets the user stored in the session.
        /// </summary>
        /// <returns></returns>
        public User GetUser()
        {
            return _session.GetObject<User>(UserKey);
        }

        /// <summary>
        /// Sets the session object with data.
        /// </summary>
        /// <param name="user"></param>
        public void SetUser(User user) 
        {
            _session.SetObject<User>(UserKey, user);    
        }
        /// <summary>
        /// Clears the session object.
        /// </summary>
        public void ClearSession()
        {
            _session.Clear();
        }
    }
}