﻿using Library_DataAccess;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia_DataAccess
{
    public class PostDTO
    {
        public PostDTO(int id, int userid, string? title, string? body, string? image, DateTime? createdat)
        {
            Id = id;
            UserId = userid;
            Title = title;
            Body = body;
            Image = image;
            CreatedAt = createdat;

            Aothor = clsUsersData.GetUserById(userid);
            Comments = clsCommentsData.GetCommentsByPostId(id);
            CommentsCount = Comments.Count;
        }
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CommentsCount { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? Image { get; set; }
        public DateTime? CreatedAt { get; set; }         
        public UserDTO? Aothor { get; set; }
        public List<CommentDTO> Comments { get; set; }
        
    }
    public class clsPostsData
    {
        public static List<PostDTO> GetAllPosts(int? page, int? count, ref int? LastPage)
        {
            List<PostDTO> list = new List<PostDTO>();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Posts_GetAllPosts", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                if (page != null)
                    command.Parameters.AddWithValue("@Page", page);
                if (count != null)
                    command.Parameters.AddWithValue("@Count", count);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string? s4 = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString("Image");

                        list.Add(new PostDTO(
                        reader.GetInt32("Id"),
                        reader.GetInt32("UserId"),
                        reader.GetString("Title"),
                        reader.GetString("Body"),
                        s4,
                        reader.GetDateTime("CreatedAt")));
                    }
                }

                using (SqlCommand command2 = new SqlCommand("SP_Posts_GetLastPage", connection))
                {
                    command2.CommandType = CommandType.StoredProcedure;
                    if (count != null)
                        command2.Parameters.AddWithValue("@Count", count);

                    SqlParameter OutParam = new SqlParameter("@LastPage", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command2.Parameters.Add(OutParam);
                    
                             
                    command2.ExecuteNonQuery();
                    
                    LastPage = (int)OutParam.Value;
                }
            }
            return list;
        }



        //public static List<PostDTO> GetAllPosts(int? page, int? count, ref int? MaxPage)
        //{
        //    List<PostDTO> list = new List<PostDTO>();

        //    using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        //    using (SqlCommand command = new SqlCommand("SP_Posts_GetAllPosts", connection))
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        if (page != null)
        //            command.Parameters.AddWithValue("@Page", page);
        //        if (count != null)
        //            command.Parameters.AddWithValue("@Count", count);

        //        SqlParameter OutParam = new SqlParameter("@MaxPage", SqlDbType.Int)
        //        {
        //            Direction = ParameterDirection.Output
        //        };
        //        command.Parameters.Add(OutParam);


        //        connection.Open();

        //        command.ExecuteNonQuery();


        //        MaxPage = (int)OutParam.Value;
        //    }
        //    return list;
        //}






        public static PostDTO? GetPostById(int Id)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Posts_GetPostByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", Id);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string? s4 = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString("Image");

                        return new PostDTO(
                        reader.GetInt32("Id"),
                        reader.GetInt32("UserId"),
                        reader.GetString("Title"),
                        reader.GetString("Body"),
                        s4,
                        reader.GetDateTime("CreatedAt"));
                    }
                }
            }
            return null;
        }
        public static int AddNewPost(PostDTO dto)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Posts_AddNewPost", connection))
            {
                command.CommandType = CommandType.StoredProcedure; command.Parameters.AddWithValue("@UserId", dto.UserId);
                command.Parameters.AddWithValue("@Title", dto.Title);
                command.Parameters.AddWithValue("@Body", dto.Body);
                command.Parameters.AddWithValue("@Image", (string.IsNullOrEmpty(dto.Image) ? DBNull.Value : dto.Image));
                if (dto.CreatedAt != null)
                    command.Parameters.AddWithValue("@CreatedAt", dto.CreatedAt);

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
        public static bool UpdatePost(PostDTO dto)
        {
            int AffectedRows = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Posts_UpdatePost", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", dto.Id);
                command.Parameters.AddWithValue("@UserId", dto.UserId);
                command.Parameters.AddWithValue("@Title", dto.Title);
                command.Parameters.AddWithValue("@Body", dto.Body);
                command.Parameters.AddWithValue("@Image", (string.IsNullOrEmpty(dto.Image) ? DBNull.Value : dto.Image));
                command.Parameters.AddWithValue("@CreatedAt", dto.CreatedAt);

                connection.Open();
                AffectedRows = command.ExecuteNonQuery();

            }
            return AffectedRows != 0;
        }
        public static bool IsPostExist(int Id)
        {
            int IsFound = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_Posts_IsPostExist", connection))
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
        public static bool DeletePost(int? Id)
        {
            if (Id == null)
                return false;
            int AffectedRows = 0;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SP_Posts_DeletePost", connection))
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
