using Library_DataAccess;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia_DataAccess
{
    public class UserDTO
    {
        public UserDTO(int id, string? name, string? email, string? username, string? password, string? image)
        {
            Id = id;
            Name = name;
            Email = email;
            UserName = username;
            Password = password;
            Image = image;
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Image { get; set; }
    }
    public class clsUsersData
    {
        public static List<UserDTO> GetAllUsers()
        {
            List<UserDTO> list = new List<UserDTO>();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Users_GetAllUsers", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string? s2 = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString("Email");

                        string? s5 = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString("Image");

                        list.Add(new UserDTO(
                        reader.GetInt32("Id"),
                        reader.GetString("Name"),
                        s2,
                        reader.GetString("UserName"),
                        reader.GetString("Password"),
                        s5));
                    }
                }
            }
            return list;
        }
        public static UserDTO? GetUserById(int Id)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Users_GetUserByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", Id);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string? s2 = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString("Email");

                        string? s5 = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString("Image");

                        return new UserDTO(
                        reader.GetInt32("Id"),
                        reader.GetString("Name"),
                        s2,
                        reader.GetString("UserName"),
                        reader.GetString("Password"),
                        s5);
                    }
                }
            }
            return null;
        }
        public static UserDTO? GetUserByUserName(string UserName)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Users_GetUserByUserName", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserName", UserName);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string? s2 = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString("Email");

                        string? s5 = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString("Image");

                        return new UserDTO(
                        reader.GetInt32("Id"),
                        reader.GetString("Name"),
                        s2,
                        reader.GetString("UserName"),
                        reader.GetString("Password"),
                        s5);
                    }
                }
            }
            return null;
        }
        public static int AddNewUser(UserDTO dto)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Users_AddNewUser", connection))
            {
                command.CommandType = CommandType.StoredProcedure; command.Parameters.AddWithValue("@Name", dto.Name);
                command.Parameters.AddWithValue("@Email", (string.IsNullOrEmpty(dto.Email) ? DBNull.Value : dto.Email));
                command.Parameters.AddWithValue("@UserName", dto.UserName);
                command.Parameters.AddWithValue("@Password", dto.Password);
                command.Parameters.AddWithValue("@Image", (string.IsNullOrEmpty(dto.Image) ? DBNull.Value : dto.Image));

                SqlParameter OutParam = new SqlParameter("@Id", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(OutParam);

                connection.Open();
                command.ExecuteNonQuery();

                return (int)OutParam.Value;
            }
        }
        public static bool UpdateUser(UserDTO dto)
        {
            int AffectedRows = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Users_UpdateUser", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", dto.Id);
                command.Parameters.AddWithValue("@Name", dto.Name);
                command.Parameters.AddWithValue("@Email", (string.IsNullOrEmpty(dto.Email) ? DBNull.Value : dto.Email));
                command.Parameters.AddWithValue("@UserName", dto.UserName);
                command.Parameters.AddWithValue("@Password", dto.Password);
                command.Parameters.AddWithValue("@Image", (string.IsNullOrEmpty(dto.Image) ? DBNull.Value : dto.Image));

                connection.Open();
                AffectedRows = command.ExecuteNonQuery();

            }
            return AffectedRows != 0;
        }
        public static bool IsUserExist(int Id)
        {
            int IsFound = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Users_IsUserExist", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", Id);

                SqlParameter OutParam = new SqlParameter("@Out", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(OutParam);

                connection.Open();
                command.ExecuteNonQuery();
                IsFound = (int)OutParam.Value;
            }
            return IsFound != 0;
        }
        public static bool IsUserExist(string UserName)
        {
            int IsFound = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Users_IsUserExistByUserName", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserName", UserName);

                SqlParameter OutParam = new SqlParameter("@Out", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(OutParam);

                connection.Open();
                command.ExecuteNonQuery();
                IsFound = (int)OutParam.Value;
            }
            return IsFound != 0;
        }
        public static bool DeleteUser(int? Id)
        {
            if (Id == null)
                return false;
            int AffectedRows = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SP_Users_DeleteUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", Id);

                    AffectedRows = command.ExecuteNonQuery();
                }
            }
            return AffectedRows != 0;
        }
    }


}
