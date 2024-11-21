using ContosoPizza.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ContosoPizza.Utilities.JWT
{
    public class JWTProvider : IJWTProvider
    {
        private readonly JwtOption _option;
        public JWTProvider(IOptions<JwtOption> option)
        {
            _option = option.Value;
        }
        public string GenerateToken(Customer customer)
        {
            // Создаем массив Claims
            Claim[] claims = new Claim[]
            {
                new Claim("cartId", customer.CartId.ToString()!),
                new Claim("role", customer.Role.Name) // Если Role или Name == null, будет использоваться "defaultRole"
            };

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_option.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            // Создание токена.
            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials, // Алгоритм кодировки токена.
                expires: DateTime.UtcNow.AddHours(_option.ExpiresHours)
                );

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token); // Преобразование (сериализация) объекта token
                                                                              // класса JwtSecurityToken в строку (string).

            return tokenValue;
        }
    }
}
