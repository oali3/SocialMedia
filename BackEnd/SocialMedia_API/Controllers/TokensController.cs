using Microsoft.AspNetCore.Mvc;
using SocialMedia_Bussiness;
using SocialMedia_DataAccess;

namespace SocialMedia_API.Controllers
{

    //[Route("api/Tokens")]
    //[ApiController]
    //public class TokensController : ControllerBase
    //{
    //    [HttpGet(Name = "GetAllToken")]
    //    [ProducesResponseType(StatusCodes.Status200OK)]
    //    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //    public ActionResult<IEnumerable<TokenDTO>> GetAllTokens()
    //    {
    //        List<TokenDTO> list = clsToken.GetAllTokens(); if (list.Count == 0)
    //            return NotFound("No Token Found");
    //        return Ok(list);
    //    }
    //    [HttpGet("{id}", Name = "GetTokenById")]
    //    [ProducesResponseType(StatusCodes.Status200OK)]
    //    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //    public ActionResult<IEnumerable<TokenDTO>> GetTokenById(int id)
    //    {
    //        if (id < 1)
    //            return BadRequest("Not valid ID");

    //        clsToken? token = clsToken.Find(id);

    //        if (token == null)
    //            return NotFound($"Token with Id({id}) Not Found");

    //        return Ok(token.DTO);
    //    }
    //    [HttpPost(Name = "AddToken")]
    //    [ProducesResponseType(StatusCodes.Status201Created)]
    //    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //    public ActionResult<IEnumerable<TokenDTO>> AddNewToken(TokenDTO dto)
    //    {
    //        if (dto == null || string.IsNullOrEmpty(dto.Token))
    //            return BadRequest("Invalid Token data");

    //        clsToken? token = new clsToken(dto);

    //        token.Save();

    //        return CreatedAtRoute("AddToken", token.DTO);
    //    }

    //    [HttpPut("{id}", Name = "UpdateToken")]
    //    [ProducesResponseType(StatusCodes.Status200OK)]
    //    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //    public ActionResult<IEnumerable<TokenDTO>> UpdateToken(int id, TokenDTO dto)
    //    {
    //        if (dto == null || string.IsNullOrEmpty(dto.Token))
    //            return BadRequest("Invalid Token data");

    //        clsToken? token = clsToken.Find(id);


    //        if (token == null)
    //            return NotFound($"Token with Id({id}) Not Found");

    //        token.Token = dto.Token;

    //        token.Save();

    //        return Ok("Token Updated Successfully");
    //    }

    //    [HttpDelete("{id}", Name = "DeleteToken")]
    //    [ProducesResponseType(StatusCodes.Status200OK)]
    //    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //    public ActionResult<IEnumerable<TokenDTO>> DeleteToken(int id)
    //    {
    //        if (id < 1)
    //            return BadRequest("Invalid Id");

    //        if (!clsToken.IsTokenExist(id))
    //            return NotFound($"Token with Id({id}) not Found");

    //        clsToken.DeleteToken(id);

    //        return Ok("Token Deleted Successfully");
    //    }


    //}



}
