using SocialMedia_DataAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia_Bussiness
{
    public class clsPost
    {
        public enum enMode { AddNew, Update };
        public enMode Mode;

        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? Image { get; set; }
        public DateTime? CreatedAt { get; set; }
        public PostDTO DTO { get { return new PostDTO(Id, UserId, Title, Body, Image, CreatedAt); } }

        public clsPost(PostDTO dto, enMode mode = enMode.AddNew)
        {
            Id = dto.Id;
            UserId = dto.UserId;
            Title = dto.Title;
            Body = dto.Body;
            Image = dto.Image;
            CreatedAt = dto.CreatedAt;

            Mode = mode;
        }


        public static List<PostDTO> GetAllPosts(int? page, int? count, ref int? LastPage)
        {
            return clsPostsData.GetAllPosts(page, count, ref LastPage);
        }

        public static clsPost? Find(int Id)
        {
            PostDTO? Dto = clsPostsData.GetPostById(Id);
            if (Dto != null)
                return new clsPost(Dto, enMode.Update);
            else return null;
        }
        private bool _AddNewPost()
        {
            Id = clsPostsData.AddNewPost(DTO);
            return Id != -1;
        }
        private bool _UpdatePost()
        {
            return clsPostsData.UpdatePost(DTO);
        }


        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPost())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    if (_UpdatePost())
                        return true;
                    else
                        return false;
            }
            return false;
        }

        public static bool IsPostExist(int Id)
        {
            return clsPostsData.IsPostExist(Id);
        }

        public static bool DeletePost(int Id)
        {
            return clsPostsData.DeletePost(Id);
        }

    }

}
