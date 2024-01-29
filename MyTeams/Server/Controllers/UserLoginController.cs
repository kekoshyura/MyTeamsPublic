using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyTeams.Client;
using MyTeams.Server.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyTeams.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserLoginController : ControllerBase {

    [HttpPost]
    public async Task<ActionResult<string>>
    Login(UserInput userInput) {
        if (userInput.UserName != "admin")
            return BadRequest();
        if (userInput.Password != "adminteamscore")
            return BadRequest();

        var claims = new List<Claim> {
            new Claim(ClaimTypes.Name, userInput.UserName)
        };
        var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        var tokenString = new JwtSecurityTokenHandler().WriteToken(jwt);
        HttpContext.Response.Cookies.Append("token", tokenString, new CookieOptions(){Expires = DateTimeOffset.MaxValue});
        return tokenString;
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<ActionResult<string>>
    FromCookies() {
        var res = HttpContext.Request.Cookies;
        var token = res["token"];
        if (token == null)
            return BadRequest();
        return token;
    }
}