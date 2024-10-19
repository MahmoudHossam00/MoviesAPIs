
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication2.Helpers;

namespace WebApplication2.Services
{
	public class AuthServices : IAuthServices
	{


		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly JWT _jwt;


		public AuthServices(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_jwt = jwt.Value;
		}

		public async Task<string> AddRoleAsync(AddRoleModel model)
		{
			var user = await _userManager.FindByIdAsync(model.UserId);
			if (user == null)
			{
				return "Invalid User Id";
			}
			if (!await _roleManager.RoleExistsAsync(model.Role)) {
				return "Role Is Invalid";
					}

			if(await _userManager.IsInRoleAsync(user,model.Role))
			{
				return "User Is already assigned to this rule";
			}

			var result = await _userManager.AddToRoleAsync(user, model.Role);

			return result.Succeeded ? string.Empty : "SomethingWentWrong";
		}

		public async Task<AuthModel> GetTokenAsync(TokenRequestModel model)
		{
			var authModel= new AuthModel();
			var user=await _userManager.FindByEmailAsync(model.Email);
			if (user == null|| !await _userManager.CheckPasswordAsync(user,model.Password))
			{
				authModel.Message = "the email/password is incorrect";
				return authModel;
			}
			var jwtSecurityToken = await CreateJwtToken(user);
			authModel.IsAuthenticated = true;
			authModel.Email = user.Email;
			authModel.UserName = user.UserName;
			authModel.ExpiresOn = jwtSecurityToken.ValidTo;
			authModel.Token=new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
			var rolesList = await _userManager.GetRolesAsync(user);
			authModel.Roles = rolesList.ToList();

			
			return authModel;
		}

		public async Task<AuthModel> RegisterAsync(RegisterModel model)
		{
			if(await _userManager.FindByEmailAsync(model.Email) is not null)
			{
				return new AuthModel { Message = "Email Is Already Registered" };
			}
			if(await _userManager.FindByNameAsync(model.Username) is not null)
			{
				return new AuthModel { Message = "Username is taken " };
			}
			
			var user = new ApplicationUser { 
				UserName = model.Username,
				Email = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName,

			};
			var result =await _userManager.CreateAsync(user,model.Password);
			if (!result.Succeeded)
			{
				string errors = string.Empty;
				foreach (var error in result.Errors)
				{
					errors += $"{error.Description}";
				}
				return new AuthModel { Message=errors};
			}

			await _userManager.AddToRoleAsync(user, RolesStrings._user);
			var jwtToken = await CreateJwtToken(user);
			return new AuthModel
			{
				Email = user.Email,
				ExpiresOn = jwtToken.ValidTo,
				IsAuthenticated = true,
				Roles = new List<string> { RolesStrings._user },
				Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
				UserName = user.UserName
			};


		}

		private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
		{
			var userClaims = await _userManager.GetClaimsAsync(user);
			var roles = await _userManager.GetRolesAsync(user);
			var RoleClaims = new List<Claim>();
			foreach (var role in roles)
			{
				RoleClaims.Add(new Claim("roles", role));
			}
			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
				new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Email,user.Email),
				new Claim("uid",user.Id),
			}.Union(userClaims).Union(RoleClaims);
			var SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
			var SigningCredentialss = new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256);

			var jwtSecurityToken = new JwtSecurityToken

				(

				issuer: _jwt.Issuer,
				audience: _jwt.Audience,
				claims: claims,
				expires: DateTime.UtcNow.AddDays(_jwt.DurationInDays),
				signingCredentials: SigningCredentialss
				);

			return jwtSecurityToken;
			
			
			
		}
	}
}
