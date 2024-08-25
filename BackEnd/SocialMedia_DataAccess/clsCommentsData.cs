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
    public class CommentDTO
    {
        public CommentDTO(int id, int userid, int postid, string? text)
        {
            Id = id;
            UserId = userid;
            PostId = postid;
            Text = text;

            User = clsUsersData.GetUserById(userid);
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string? Text { get; set; }
        public UserDTO? User { get; set; }
    }
    public class clsCommentsData
    {
        public static int GetCommentsCount(int PostId)
        {
            int Count = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Posts_GetCommentsCount", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PostId", PostId);

                connection.Open();

                Count = (int)command.ExecuteScalar();
            }
            return Count;
        }

        public static List<CommentDTO> GetCommentsByPostId(int postId)
        {
            List<CommentDTO> list = new List<CommentDTO>();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Comments_GetCommentsByPostId", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("PostId", postId);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new CommentDTO(
                        reader.GetInt32("Id"),
                        reader.GetInt32("UserId"),
                        reader.GetInt32("PostId"),
                        reader.GetString("Text")));
                    }
                }
            }
            return list;
        }


        public static List<CommentDTO> GetAllComments()
        {
            List<CommentDTO> list = new List<CommentDTO>();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Comments_GetAllComments", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new CommentDTO(
                        reader.GetInt32("Id"),
                        reader.GetInt32("UserId"),
                        reader.GetInt32("PostId"),
                        reader.GetString("Text")));
                    }
                }
            }
            return list;
        }
        public static CommentDTO? GetCommentById(int Id)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Comments_GetCommentByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", Id);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new CommentDTO(
                        reader.GetInt32("Id"),
                        reader.GetInt32("UserId"),
                        reader.GetInt32("PostId"),
                        reader.GetString("Text"));
                    }
                }
            }
            return null;
        }
        public static int AddNewComment(CommentDTO dto)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Comments_AddNewComment", connection))
            {
                command.CommandType = CommandType.StoredProcedure; command.Parameters.AddWithValue("@UserId", dto.UserId);
                command.Parameters.AddWithValue("@PostId", dto.PostId);
                command.Parameters.AddWithValue("@Text", dto.Text);

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
        public static bool UpdateComment(CommentDTO dto)
        {
            int AffectedRows = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Comments_UpdateComment", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", dto.Id);
                command.Parameters.AddWithValue("@UserId", dto.UserId);
                command.Parameters.AddWithValue("@PostId", dto.PostId);
                command.Parameters.AddWithValue("@Text", dto.Text);

                connection.Open();
                AffectedRows = command.ExecuteNonQuery();

            }
            return AffectedRows != 0;
        }
        public static bool IsCommentExist(int Id)
        {
            int IsFound = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Comments_IsCommentExist", connection))
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
        public static bool DeleteComment(int? Id)
        {
            if (Id == null)
                return false;
            int AffectedRows = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SP_Comments_DeleteComment", connection))
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
