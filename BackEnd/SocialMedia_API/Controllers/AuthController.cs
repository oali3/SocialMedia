using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia_Bussiness;
using SocialMedia_DataAccess;

namespace SocialMedia_API.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("Register", Name = "RegisterUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<UserDTO>> AddNewUser(UserDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.UserName) || string.IsNullOrEmpty(dto.Password))
                return BadRequest("Missing UserName or Password");


            if (clsUser.IsUserExist(dto.UserName))
                return BadRequest("User Name is already exist");

            clsUser? user = new clsUser(dto);

            user.Save();

            string? tk = Guid.NewGuid().ToString("N").Substring(0, 16);


            clsToken Token = new clsToken(new TokenDTO(user.Id, tk));

            Token.Save();

            HttpContext.Response.Headers.Append("Authorization", $"Bearer {Token.Token}");

            return CreatedAtRoute("RegisterUser", new { Id = Token.UserId, token = Token.Token });
        }


        [HttpPost("Login", Name = "Login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<UserDTO>> Login(UserDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.UserName) || string.IsNullOrEmpty(dto.Password))
                return BadRequest("Missing UserName or Password");

            clsUser? user = clsUser.Find(dto.UserName);

            if (user == null)
                return NotFound("User Not Found");

            if (user?.Password != dto.Password)
                return BadRequest("Password is incorrect");

            clsToken? Token = clsToken.Find(user.Id);

            if (Token == null)
            {
                string? tk = Guid.NewGuid().ToString("N").Substring(0, 16);

                Token = new clsToken(new TokenDTO(user.Id, tk));

                Token.Save();
            }

            HttpContext.Response.Headers.Append("Authorization", $"Bearer {Token.Token}");

            return CreatedAtRoute("RegisterUser", new { token = Token.Token });
        }


    }
}
