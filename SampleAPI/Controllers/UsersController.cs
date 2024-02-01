using AutoMapper;
using Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using DALEntity = Data.Entities;
using DTO = Data.DTOs;

namespace SampleAPI.Controllers
{
    [Produces("application/json", "application/xml")]
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Users")]
    [ApiExplorerSettings(GroupName = "5. Users")]    
    public class UsersController : ControllerBase
    {  
        private readonly IRegisteredUserRepository _repo;       
        private readonly IMapper _mapper;

        public UsersController(IRegisteredUserRepository repo, IMapper mapper)
        {            
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a list of user roles
        /// </summary>
        /// <returns>An ActionResult of type IEnumerable of UserRole</returns>
        [HttpGet("userroles", Name = "GetUserRoles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Data.Entities.UserRole>>> GetUserRoles()
        {
            var userRolesFromRepo = await _repo.GetUserRolesAsync();
            return Ok(_mapper.Map<IEnumerable<Data.Entities.UserRole>>(userRolesFromRepo));
        }

        /// <summary>
        /// Get a list of registered users
        /// </summary>
        /// <returns>An ActionResult of type IEnumerable of RegisteredUser</returns>
        [HttpGet(Name = "GetRegisteredUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Data.Entities.RegisteredUser>>> GetRegisteredUsers()
        {
            var UsersFromRepo = await _repo.GetRegisteredUsersAsync();
            return Ok(_mapper.Map<List<Data.Entities.RegisteredUser>>(UsersFromRepo));
        }

        /// <summary>
        /// Get an Registered User by Id
        /// </summary>
        /// <param name="Id">The id of the registered user you want to get</param>
        /// <returns>An ActionResult of type Fund</returns>
        [HttpGet("{Id}", Name = "GetRegisteredUser")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Data.Entities.RegisteredUser>> GetRegisteredUser(int Id)
        {
            var UserFromRepo = await _repo.GetRegisteredUserAsync(Id);
            if (UserFromRepo == null) { return NotFound(); }
            return Ok(_mapper.Map<Data.Entities.RegisteredUser>(UserFromRepo));
        }

        /// <summary>
        /// Create a Registered User
        /// </summary>
        /// <param name="RegisteredUser"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /RegisteredUser
        ///     {
        ///       "userId": "myUserId",     
        ///       "userPassword": "mypassword",   
        ///       "userName": "My User Name",
        ///       "userEmail": "myemail@gmail.com",                          
        ///       "userRoleId": 2
        ///     }
        ///
        /// </remarks>
        [HttpPost(Name = "CreateRegisteredUser")]
        [Consumes("application/json")]
        public async Task<ActionResult<Data.Entities.RegisteredUser>> CreateFund(DTO.RegisteredUserForCreation RegisteredUser)
        {
            var entity = _mapper.Map<DALEntity.RegisteredUser>(RegisteredUser);
            entity.ProductId = Globals.ApplicationProductId; // this was set in authorize in swagger or in request header
            entity.NumberOfLogins = 1;
            entity.LastLogin = DateTime.Today;
            _repo.AddRegisteredUser(entity);
            await _repo.SaveChangesAsync();

            var UserToReturn = _mapper.Map<DTO.RegisteredUser>(entity);
            return CreatedAtRoute("GetRegisteredUser", new { Id = entity.Id }, UserToReturn);
        }

        /// <summary>
        /// Updates a Registered User by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="RegisteredUser"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /RegisteredUser
        ///     {
        ///       "userId": "myUserId",     
        ///       "userPassword": "mypassword",   
        ///       "userName": "My User Name",
        ///       "userEmail": "myemail@gmail.com",                          
        ///       "userRoleId": 2
        ///     }
        ///
        /// </remarks>
        [HttpPut("{Id}", Name = "UpdateRegisteredUser")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateRegisteredUser(int Id, DTO.RegisteredUserForCreation RegisteredUser)
        {
            // put requires all input fields.
            var RegisteredUserFromRepo = await _repo.GetRegisteredUserAsync(Id);
            if (RegisteredUserFromRepo == null) { return NotFound(); }
            _mapper.Map(RegisteredUser, RegisteredUserFromRepo);
            _repo.UpdateRegisteredUser(RegisteredUserFromRepo);
            await _repo.SaveChangesAsync();
            return Ok(_mapper.Map<Data.Entities.RegisteredUser>(RegisteredUserFromRepo));           
        }

        /// <summary>
        /// Deletes an RegisteredUser by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("{Id}", Name = "DeleteRegisteredUser")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteRegisteredUser(int Id)
        {
            var RegisteredUserToDelete = await _repo.GetRegisteredUserAsync(Id);
            if (RegisteredUserToDelete == null)
            {
                return NotFound($"RegisteredUser with Id = {Id} not found");
            }
            await _repo.DeleteRegisteredUser(Id);
            return NoContent();
        }
    }
}