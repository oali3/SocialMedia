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
    public class TokenDTO
    {
        public TokenDTO(int userid, string? token)
        {
            UserId = userid;
            Token = token;
        }

        public int UserId { get; set; }
        public string? Token { get; set; }
    }
    public class clsTokensData
    {
        public static List<TokenDTO> GetAllTokens()
        {
            List<TokenDTO> list = new List<TokenDTO>();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Tokens_GetAllTokens", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new TokenDTO(
                        reader.GetInt32("UserId"),
                        reader.GetString("Token")));
                    }
                }
            }
            return list;
        }
        public static TokenDTO? GetTokenById(int UserId)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Tokens_GetTokenByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", UserId);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new TokenDTO(
                        reader.GetInt32("UserId"),
                        reader.GetString("Token"));
                    }
                }
            }
            return null;
        }
        public static TokenDTO? GetTokenByToken(string Token)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Tokens_GetTokenByToken", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Token", Token);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new TokenDTO(
                        reader.GetInt32("UserId"),
                        reader.GetString("Token"));
                    }
                }
            }
            return null;
        }
        public static bool AddNewToken(TokenDTO dto)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Tokens_AddNewToken", connection))
            {
                command.CommandType = CommandType.StoredProcedure; command.Parameters.AddWithValue("@UserId", dto.UserId);
                command.CommandType = CommandType.StoredProcedure; command.Parameters.AddWithValue("@Token", dto.Token);

                connection.Open();
                int AffectedRows = command.ExecuteNonQuery();

                return AffectedRows != 0;
            }
        }
        public static bool UpdateToken(TokenDTO dto)
        {
            int AffectedRows = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Tokens_UpdateToken", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", dto.UserId);
                command.Parameters.AddWithValue("@Token", dto.Token);

                connection.Open();
                AffectedRows = command.ExecuteNonQuery();

            }
            return AffectedRows != 0;
        }
        public static bool IsTokenExist(int UserId)
        {
            int IsFound = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Tokens_IsTokenExist", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", UserId);

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
        public static bool DeleteToken(int? UserId)
        {
            if (UserId == null)
                return false;
            int AffectedRows = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SP_Tokens_DeleteToken", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@UserId", UserId);

                    AffectedRows = command.ExecuteNonQuery();
                }
            }
            return AffectedRows != 0;
        }
    }

}
