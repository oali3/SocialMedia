using Microsoft.AspNetCore.Mvc;
using SocialMedia_Bussiness;
using SocialMedia_DataAccess;

namespace SocialMedia_API.Controllers
{
    [Route("api/Posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        [HttpGet(Name = "GetAllPost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<PostDTO>> GetAllPosts()
        {
            List<PostDTO> list = clsPost.GetAllPosts(); if (list.Count == 0)
                return NotFound("No Post Found");
            return Ok(list);
        }
        [HttpGet("{id}", Name = "GetPostById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<PostDTO>> GetPostById(int id)
        {
            if (id < 1)
                return BadRequest("Not valid ID");

            clsPost? post = clsPost.Find(id);

            if (post == null)
                return NotFound($"Post with Id({id}) Not Found");

            return Ok(post.DTO);
        }
        [HttpPost(Name = "AddPost")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<PostDTO>> AddNewPost(PostDTO dto)
        {
            if (dto == null || dto.UserId <= 0 || string.IsNullOrEmpty(dto.Title) || string.IsNullOrEmpty(dto.Body))
                return BadRequest("Invalid Post data");

            clsPost? post = new clsPost(dto);

            post.Save();

            return CreatedAtRoute("AddPost", post.DTO);
        }

        [HttpPut("{id}", Name = "UpdatePost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<PostDTO>> UpdatePost(int id, PostDTO dto)
        {
            if (dto == null || dto.UserId <= 0 || string.IsNullOrEmpty(dto.Title) || string.IsNullOrEmpty(dto.Body))
                return BadRequest("Invalid Post data");

            clsPost? post = clsPost.Find(id);


            if (post == null)
                return NotFound($"Post with Id({id}) Not Found");

            post.UserId = dto.UserId;
            post.Title = dto.Title;
            post.Body = dto.Body;
            post.Image = dto.Image;

            post.Save();

            return Ok("Post Updated Successfully");
        }

        [HttpDelete("{id}", Name = "DeletePost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<PostDTO>> DeletePost(int id)
        {
            if (id < 1)
                return BadRequest("Invalid Id");

            if (!clsPost.IsPostExist(id))
                return NotFound($"Post with Id({id}) not Found");

            clsPost.DeletePost(id);

            return Ok("Post Deleted Successfully");
        }


    }

}
