using Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services;
using Services.Logger;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;

namespace SampleAPI.Controllers
{
    [Consumes(MediaTypeNames.Application.JsonPatch)]
    [ApiController]
    [ApiExplorerSettings(GroupName = "1. Authentication")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggerManager _logger;       

        public class AuthenticationRequestBody
        {
            public string? UserName { get; set; }
            public string? Password { get; set; }
            public string? ProductId { get; set; }
        }
        private class ApiUser
        {
            public int UserIdKey { get; set; }
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string UserEmail { get; set; }
            public string UserPassword { get; set; }
            public int UserRoleId { get; set; }
            public string UserRole { get; set; }
            public string OrganizationName { get; set; }
            public ApiUser(
                int userIdKey,
                string userName,
                string userEmail,
                string userId,
                int userRoleId,
                string userRole,
                string organizationName
            )
            {
                UserIdKey = userIdKey;
                UserName = userName;
                UserEmail = userEmail;
                UserId = userId;
                UserRoleId = userRoleId;
                UserRole = userRole;
                OrganizationName = organizationName;
            }
        }
        public AuthenticationController(IConfiguration configuration, ILoggerManager logger, IFundRepository repo)
        {
            _configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));
            _logger = logger;            
        }
        private ApiUser ValidateUserCredentials(string userName, string password, string productId)
        {
            int userIdKey = 0; int userRoleId = 0;
            string userFullName = string.Empty;
            string userEmailAddress = string.Empty;
            var repo = new Data.Repository.RegisteredUser(productId);
            var isAuthorized = repo.IsValidUser(userName, password, ref userRoleId, ref Globals.ApplicationUserRole, ref Globals.ApplicationOrganizationName,
                                 ref userIdKey, ref userFullName, ref userEmailAddress);
            if (repo.HasException) { throw repo.ClassException; }
            if (isAuthorized)
            {
                return new ApiUser(userIdKey, userFullName, userEmailAddress, userName, userRoleId, Globals.ApplicationUserRole, Globals.ApplicationOrganizationName);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Get API Security token for valid user and application product id
        /// </summary>
        /// <param name="authenticationRequestBody"></param>
        /// <returns>An ActionResult of type string containing authentication token</returns>  
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /AuthenticationRequestBody
        ///         "userName": "admin",
        ///         "password": "admin",
        ///         "productId": "5000A-400-3000B"
        ///
        /// </remarks>
        [HttpPost(Name = "GetUserAuthenticationToken")]
        public ActionResult<string> Authenticate(AuthenticationRequestBody authenticationRequestBody)
        {
            // Step 1: validate the username/password
            var user = ValidateUserCredentials(
                authenticationRequestBody.UserName,
                authenticationRequestBody.Password,
                authenticationRequestBody.ProductId);

            if (user == null)
            {
                _logger.LogWarn($"Unauthorized for Product Id {authenticationRequestBody.ProductId}");
                return Unauthorized();
            }
            Globals.ApplicationProductId = authenticationRequestBody.ProductId;
            Globals.ApplicationUserRole = user.UserRole;
            Globals.ApplicationOrganizationName = user.OrganizationName;

            // Step 2: create a token
            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));
            var signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            // store claims for later access by decoding the claim values from the token string.
            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("user_id_key", user.UserIdKey.ToString()));
            claimsForToken.Add(new Claim("user_name", user.UserName));
            claimsForToken.Add(new Claim("user_email", user.UserEmail));
            claimsForToken.Add(new Claim("user_id", user.UserId));
            claimsForToken.Add(new Claim("user_password", authenticationRequestBody.Password));
            claimsForToken.Add(new Claim("user_role_id", user.UserRoleId.ToString()));
            claimsForToken.Add(new Claim("user_role", user.UserRole));
            claimsForToken.Add(new Claim("organization_name", user.OrganizationName));
            claimsForToken.Add(new Claim("product_id", authenticationRequestBody.ProductId));

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler()
               .WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn);
        }
    }
}