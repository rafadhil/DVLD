using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class User
    {
        public enum enMode { AddNew, Update, Delete }
        public int UserID { set; get; }
        public Person Person { set; get; }
        public String Username { set; get; }
        public String Password { set; private get; }
        public bool IsActive { set; get; }
        private enMode Mode;

        public User()
        {
            Mode = enMode.AddNew;
            UserID = -1;
            Person = null;
            Username = "";
            Password = "";
            IsActive = false;
        }

        private User(int UserID, Person person, String Username, String Password, bool IsActive)
        {
            this.UserID = UserID;
            this.Person = person;
            this.Username = Username;
            this.Password = Password;
            this.IsActive = IsActive;
            this.Mode = enMode.Update;
        }

        public bool ChangePassword(String CurrentPassword, String NewPassword)
        {
            if(ValidateLoginCredentials(this.Username, CurrentPassword))
            {
                return UserData.ChangePassword(this.UserID, NewPassword);
            }
            else
            {
                return false;
            }
        }
        public bool ValidateLoginCredentials()
        {
            return ValidateLoginCredentials(this.Username, this.Password);
        }

        public static bool ValidateLoginCredentials(String Username, String Password)
        {
            return UserData.ValidateLoginCredentials(Username, Password);
        }

        public static DataTable GetAllUsers()
        {
            return UserData.GetAllUsers();
        }
        public static DataTable GetActiveUsers()
        {
            return UserData.GetActiveUsers();

        }
        public static DataTable GetInactiveUsers()
        {
            return UserData.GetInactiveUsers();

        }

        public static DataTable GetUsersByUserIDLike(int UserID)
        {
            return UserData.GetUsersByUserIDLike(UserID);

        }
        public static DataTable GetUsersByPersonIDLike(int PersonID)
        {
            return UserData.GetUsersByPersonIDLike(PersonID);

        }

        public static User GetUserByUserID(int UserID)
        {
            int PersonID = -1;
            String Username = "";
            bool IsActive = false;

            if (UserData.GetUserByUserID(UserID, ref PersonID, ref Username, ref IsActive))
            {
                return new User(UserID, Person.GetPersonByID(PersonID), Username, "", IsActive);
            }
            else
            {
                return null;
            }

        }

        public static User GetUserByUsername(String Username)
        {
            int UserID = -1;
            int PersonID = -1;
            bool IsActive = false;

            if (UserData.GetUserByUsername(Username , ref UserID , ref PersonID, ref IsActive))
            {
                return new User(UserID, Person.GetPersonByID(PersonID), Username, "", IsActive);
            }
            else
            {
                return null;
            }
        }
        public static User GetUserByPersonID(int PersonID)
        {
            int UserID = -1;
            String Username = "";
            bool IsActive = false;

            if (UserData.GetUserByPersonID(PersonID, ref UserID , ref Username, ref IsActive))
            {
                return new User(UserID, Person.GetPersonByID(PersonID), Username, "", IsActive);
            }
            else
            {
                return null;
            }
        }
        public static DataTable GetUsersByFullNameLike(String Fullname)
        {
            return UserData.GetUsersByFullnameLike(Fullname);
        }

        public static DataTable GetUsersByUsernameLike(String Username)
        {
            return UserData.GetUsersByUsernameLike(Username);
        }

        public bool DeleteUser()
        {
            if(UserData.DeleteUser(this.UserID))
            {
                Mode = enMode.Delete;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool DeleteUser(int UserID)
        {
            return UserData.DeleteUser(UserID);
        }

        public bool Save()
        {
            if(Mode == enMode.AddNew)
            {
                UserID = UserData.AddNewUser(this.Person.PersonID, this.Username, this.Password, IsActive);
                if(UserID != -1)
                {
                    Mode = enMode.Update;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if(Mode == enMode.Update)
            {
                return UserData.UpdateUser(this.UserID, this.Username, this.Password, IsActive);
            }
            else
            {
                return false;
            }
        }
    }
}
