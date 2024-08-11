using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia_Bussiness;
using SocialMedia_DataAccess;

namespace SocialMedia_API.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet(Name = "GetAllUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<UserDTO>> GetAllUsers()
        {
            List<UserDTO> list = clsUser.GetAllUsers(); if (list.Count == 0)
                return NotFound("No User Found");
            return Ok(list);
        }
        [HttpGet("{id}", Name = "GetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<UserDTO>> GetUserById(int id)
        {
            if (id < 1)
                return BadRequest("Not valid ID");

            clsUser? user = clsUser.Find(id);

            if (user == null)
                return NotFound($"User with Id({id}) Not Found");

            return Ok(user.DTO);
        }
        [HttpPost(Name = "AddUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<UserDTO>> AddNewUser(UserDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.UserName) || string.IsNullOrEmpty(dto.Password))
                return BadRequest("Invalid User data");

            if (clsUser.IsUserExist(dto.UserName))
                return BadRequest($"User with UserName {dto.UserName} already exists");

            clsUser? user = new clsUser(dto);

            user.Save();

            return CreatedAtRoute("AddUser", user.DTO);
        }

        [HttpPut("{id}", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<UserDTO>> UpdateUser(int id, UserDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.UserName) || string.IsNullOrEmpty(dto.Password))
                return BadRequest("Invalid User data");

            if (clsUser.IsUserExist(dto.UserName))
                return BadRequest($"UserName {dto.UserName} is not available");

            clsUser? user = clsUser.Find(id);

            if (user == null)
                return NotFound($"User with Id({id}) Not Found");

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.UserName = dto.UserName;
            user.Password = dto.Password;
            user.Image = dto.Image;

            user.Save();

            return Ok("User Updated Successfully");
        }

        [HttpDelete("{id}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<UserDTO>> DeleteUser(int id)
        {
            if (id < 1)
                return BadRequest("Invalid Id");

            if (!clsUser.IsUserExist(id))
                return NotFound($"User with Id({id}) not Found");

            clsUser.DeleteUser(id);

            return Ok("User Deleted Successfully");
        }


    }


}
