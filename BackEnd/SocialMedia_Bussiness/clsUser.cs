using SocialMedia_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia_Bussiness
{
    public class clsUser
    {
        public enum enMode { AddNew, Update };
        public enMode Mode;

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Image { get; set; }
        public UserDTO DTO { get { return new UserDTO(Id, Name, Email, UserName, Password, Image); } }

        public clsUser(UserDTO dto, enMode mode = enMode.AddNew)
        {
            Id = dto.Id;
            Name = dto.Name;
            Email = dto.Email;
            UserName = dto.UserName;
            Password = dto.Password;
            Image = dto.Image;

            Mode = mode;
        }


        public static List<UserDTO> GetAllUsers()
        {
            return clsUsersData.GetAllUsers();
        }

        public static clsUser? Find(int Id)
        {
            UserDTO? Dto = clsUsersData.GetUserById(Id);
            if (Dto != null)
                return new clsUser(Dto, enMode.Update);
            else return null;
        }
        public static clsUser? Find(string UserName)
        {
            UserDTO? Dto = clsUsersData.GetUserByUserName(UserName);
            if (Dto != null)
                return new clsUser(Dto, enMode.Update);
            else return null;
        }
        private bool _AddNewUser()
        {
            Id = clsUsersData.AddNewUser(DTO);
            return Id != -1;
        }
        private bool _UpdateUser()
        {
            return clsUsersData.UpdateUser(DTO);
        }


        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    if (_UpdateUser())
                        return true;
                    else
                        return false;
            }
            return false;
        }

        public static bool IsUserExist(int Id)
        {
            return clsUsersData.IsUserExist(Id);
        }
        public static bool IsUserExist(string UserName)
        {
            return clsUsersData.IsUserExist(UserName);
        }

        public static bool DeleteUser(int Id)
        {
            return clsUsersData.DeleteUser(Id);
        }

    }

}
