using API_JWT.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API_JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        
        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserInfo _userData)
        {
            if(_userData!=null && _userData.Email!=null&& _userData.Password != null)
            {
                var user = await GetUser(_userData.Email, _userData.Password);
                //string[] user = userCompare.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                if (user != null && user.Rows.Count>0)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                        new Claim("Id", user.Rows[0]["Id"].ToString()),
                        new Claim("FirstName", user.Rows[0]["FirstName"].ToString()),
                        new Claim("LastName", user.Rows[0]["LastName"].ToString()),
                        new Claim("UserName", user.Rows[0]["UserName"].ToString()),
                        new Claim("Email", user.Rows[0]["Email"].ToString())
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);
                    //return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                    var myDict = new Dictionary<string, object>();
                    myDict["status"] = true;
                    myDict["status_message"] = "Success";
                    myDict["message"] = "Thành Công!";
                    myDict["token"] = new JwtSecurityTokenHandler().WriteToken(token);
                    return new JsonResult(myDict);
                }
                else
                {
                    var myDict = new Dictionary<string, object>();
                    myDict["status"] = false;
                    myDict["status_message"] = "Failed";
                    myDict["message"] = "Thông tin đăng nhập không hợp lệ!";
                    return BadRequest(myDict);
                }
            }
            else
            {
                return BadRequest();
            }
        }
        private async Task<DataTable> GetUser(string email,string password)
        {
            string query = @"select * from userinfo where email = @Email and password = @Password";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Default");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@Email", email);
                    myCommand.Parameters.AddWithValue("@Password", password);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }
            return table;
        }
    }
}
