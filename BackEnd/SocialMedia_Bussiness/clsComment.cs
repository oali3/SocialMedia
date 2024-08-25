using SocialMedia_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia_Bussiness
{
    public class clsComment
    {
        public enum enMode { AddNew, Update };
        public enMode Mode;

        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string? Text { get; set; }
        public CommentDTO DTO { get { return new CommentDTO(Id, UserId, PostId, Text); } }

        public clsComment(CommentDTO dto, enMode mode = enMode.AddNew)
        {
            Id = dto.Id;
            UserId = dto.UserId;
            PostId = dto.PostId;
            Text = dto.Text;

            Mode = mode;
        }


        public static List<CommentDTO> GetCommentsByPostId(int postId)
        {
            return clsCommentsData.GetCommentsByPostId(postId);
        }
        public static List<CommentDTO> GetAllComments()
        {
            return clsCommentsData.GetAllComments();
        }

        public static clsComment? Find(int Id)
        {
            CommentDTO? Dto = clsCommentsData.GetCommentById(Id);
            if (Dto != null)
                return new clsComment(Dto, enMode.Update);
            else return null;
        }
        private bool _AddNewComment()
        {
            Id = clsCommentsData.AddNewComment(DTO);
            return Id != -1;
        }
        private bool _UpdateComment()
        {
            return clsCommentsData.UpdateComment(DTO);
        }


        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewComment())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    if (_UpdateComment())
                        return true;
                    else
                        return false;
            }
            return false;
        }

        public static bool IsCommentExist(int Id)
        {
            return clsCommentsData.IsCommentExist(Id);
        }

        public static bool DeleteComment(int Id)
        {
            return clsCommentsData.DeleteComment(Id);
        }

    }

}
