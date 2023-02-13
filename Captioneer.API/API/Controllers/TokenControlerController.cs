using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Captioneer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenControlerController : ControllerBase
    {
        // GET: api/<TokenControlerController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<TokenControlerController>/5
        [HttpGet("{value}")]
        public bool Get(string? value)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            if (tokenHandler.CanReadToken(value))
            {
                var builder = WebApplication.CreateBuilder();
                var validationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"])),
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true
                };
                SecurityToken validatedToken = null;
                try
                {
                    tokenHandler.ValidateToken(value, validationParameters, out validatedToken);
                }
                catch (SecurityTokenException)
                {
                    return false;
                }
                catch (Exception e)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        // POST api/<TokenControlerController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TokenControlerController>/5
        [HttpPut]
        public void Put([FromBody] string value)
        {
        }

        // DELETE api/<TokenControlerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


    }

}
