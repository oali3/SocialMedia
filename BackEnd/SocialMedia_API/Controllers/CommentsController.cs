using Microsoft.AspNetCore.Mvc;
using SocialMedia_Bussiness;
using SocialMedia_DataAccess;

namespace SocialMedia_API.Controllers
{
    [Route("api/Comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        [HttpGet("GetByPostId/{PostId}", Name = "GetCommentsByPostId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<CommentDTO>> GetCommentsByPostId(int PostId)
        {
            if (PostId < 1)
                return BadRequest("Not valid ID");
            List<CommentDTO> list = clsComment.GetCommentsByPostId(PostId);
            if (list.Count == 0)
                return NotFound("No Comment Found");
            return Ok(list);
        }

                [HttpGet(Name = "GetAllComment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<CommentDTO>> GetAllComments()
        {
            List<CommentDTO> list = clsComment.GetAllComments(); if (list.Count == 0)
                return NotFound("No Comment Found");
            return Ok(list);
        }




        [HttpGet("{id}", Name = "GetCommentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<CommentDTO>> GetCommentById(int id)
        {
            if (id < 1)
                return BadRequest("Not valid ID");

            clsComment? comment = clsComment.Find(id);

            if (comment == null)
                return NotFound($"Comment with Id({id}) Not Found");

            return Ok(comment.DTO);
        }
        [HttpPost(Name = "AddComment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<CommentDTO>> AddNewComment(CommentDTO dto)
        {
            if (dto == null || dto.UserId <= 0 || dto.PostId <= 0 || string.IsNullOrEmpty(dto.Text))
                return BadRequest("Invalid Comment data");

            clsComment? comment = new clsComment(dto);

            comment.Save();

            return CreatedAtRoute("AddComment", comment.DTO);
        }

        [HttpPut("{id}", Name = "UpdateComment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<CommentDTO>> UpdateComment(int id, CommentDTO dto)
        {
            if (dto == null || dto.UserId <= 0 || dto.PostId <= 0 || string.IsNullOrEmpty(dto.Text))
                return BadRequest("Invalid Comment data");

            clsComment? comment = clsComment.Find(id);


            if (comment == null)
                return NotFound($"Comment with Id({id}) Not Found");

            comment.UserId = dto.UserId;
            comment.PostId = dto.PostId;
            comment.Text = dto.Text;

            comment.Save();

            return Ok("Comment Updated Successfully");
        }

        [HttpDelete("{id}", Name = "DeleteComment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<CommentDTO>> DeleteComment(int id)
        {
            if (id < 1)
                return BadRequest("Invalid Id");

            if (!clsComment.IsCommentExist(id))
                return NotFound($"Comment with Id({id}) not Found");

            clsComment.DeleteComment(id);

            return Ok("Comment Deleted Successfully");
        }


    }

}
