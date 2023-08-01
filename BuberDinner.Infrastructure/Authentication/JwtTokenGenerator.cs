using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Services;
using BuberDinner.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BuberDinner.Infrastructure.Authentication
{
	public class JwtTokenGenerator : IJwtTokengenerator
	{
		private readonly IDateTimeProvider _dateTimeProvider;
		private readonly JwtSettings _jwtSettings;

		public JwtTokenGenerator(IDateTimeProvider dateTimeProvider, IOptions<JwtSettings> jwtOptions)
		{
			_dateTimeProvider = dateTimeProvider;
			_jwtSettings = jwtOptions.Value;
		}

		public string GenerateToken(User user)
		{
			var signingCredentials = new SigningCredentials(
				new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
				SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
				new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			};

			var securityToken = new JwtSecurityToken(
				issuer: _jwtSettings.Issuer,
				expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
				claims: claims,
				signingCredentials: signingCredentials,
				audience: _jwtSettings.Audience) ;

			return new JwtSecurityTokenHandler().WriteToken(securityToken);
		}
	}
}
