using AutoMapper;
using Classes;
using Data.Entities;
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
    [Route("api/v{version:apiVersion}/Contributors")]
    [ApiExplorerSettings(GroupName = "3. Contributors")]    
    public class ContributorsController : ControllerBase
    {
        private readonly IContributorRepository _repo;      
        private readonly IMapper _mapper;

        public ContributorsController(IContributorRepository repo, IMapper mapper)
        {           
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a list of Contributors
        /// </summary>
        /// <returns>An ActionResult of type IEnumerable of Contributor</returns>
        [HttpGet(Name = "GetContributors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Data.Entities.Contributor>>> GetContributors()
        {
            var ContributorsFromRepo = await _repo.GetContributorsAsync();
            return Ok(_mapper.Map<List<Data.Entities.Contributor>>(ContributorsFromRepo));
        }

        /// <summary>
        /// Get an Contributor by Contributor Id
        /// </summary>
        /// <param name="ContributorId">The id of the Contributor you want to get</param>
        /// <returns>An ActionResult of type Contributor</returns>
        [HttpGet("{ContributorId}", Name = "GetContributor")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Data.Entities.Contributor>> GetContributor(int ContributorId)
        {
            var ContributorFromRepo = await _repo.GetContributorAsync(ContributorId);
            if (ContributorFromRepo == null) { return NotFound(); }
            return Ok(_mapper.Map<Data.Entities.Contributor>(ContributorFromRepo));
        }

        /// <summary>
        /// Get Contributions by Contributor Id
        /// </summary>
        /// <param name="ContributorId">The id of the Contributor you want to get</param>
        /// <returns>An ActionResult of List of Contributions</returns>
        [HttpGet("{ContributorId}/contributions", Name = "GetContributorContributions")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Contribution>>> GetContributorContributions(int ContributorId)
        {
            var ContributorFromRepo = await _repo.GetContributorAsync(ContributorId);
            if (ContributorFromRepo == null) { return NotFound("Contributor was not found"); }

            var ContributionsFromRepo = await _repo.GetContributionsByContributorAsync(ContributorId);
            if (ContributionsFromRepo == null) { return NotFound("There are now contributions for this contributor"); }
            return Ok(_mapper.Map<List<Contribution>>(ContributionsFromRepo));
        }

        /// <summary>
        /// Adding a new Contributor
        /// </summary>
        /// <param name="Contributor"></param>
        /// <returns></returns> 
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Contributor
        ///     {
        ///        "name": "Full Name",        
        ///        "address": "100 Main St",        
        ///        "city": "Fort Smith",
        ///        "stateCode": "AR",
        ///        "zipCode": "72908",                    
        ///        "phone": "479 555-1212",        
        ///        "email": "myemail@gmail.com",        
        ///        "Comments": "optional comments" 
        ///     }
        ///
        /// </remarks>
        [HttpPost(Name = "CreateContributor")]
        [Consumes("application/json")]
        public async Task<ActionResult<DALEntity.Contributor>> CreateContributor(DTO.ContributorForCreation Contributor)
        {
            var entity = _mapper.Map<DALEntity.Contributor>(Contributor);
            entity.ProductId = Globals.ApplicationProductId; // this was set in authorize in swagger or in request header
            _repo.AddContributor(entity);
            await _repo.SaveChangesAsync();

            var ContributorToReturn = _mapper.Map<DTO.Contributor>(entity);
            return CreatedAtRoute("GetContributor", new { ContributorId = entity.Id }, ContributorToReturn);
        }

        /// <summary>
        /// Updates an Contributor by Contributor id
        /// </summary>
        /// <param name="ContributorId"></param>
        /// <param name="Contributor"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Contributor
        ///     {
        ///        "name": "Full Name",        
        ///        "address": "100 Main St",        
        ///        "city": "Fort Smith",
        ///        "stateCode": "AR",
        ///        "zipCode": "72908",                    
        ///        "phone": "479 555-1212",     
        ///        "email": "myemail@gmail.com",        
        ///        "Comments": "optional comments" 
        ///     }
        ///
        /// </remarks>
        [HttpPut("{ContributorId}", Name = "UpdateContributor")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateContributor(int ContributorId, DTO.ContributorForCreation Contributor)
        {
            // put updates all fields for an entity so if do not supply values it will reset to default values. In this case works, only one field to update.
            var ContributorFromRepo = await _repo.GetContributorAsync(ContributorId);
            if (ContributorFromRepo == null) { return NotFound(); }
            _mapper.Map(Contributor, ContributorFromRepo);
            _repo.UpdateContributor(ContributorFromRepo);
            await _repo.SaveChangesAsync();
            return Ok(_mapper.Map<Contributor>(ContributorFromRepo)); // return the Contributor       
        }

        /// <summary>
        /// Deletes an Contributor by Contributor id
        /// </summary>
        /// <param name="ContributorId"></param>
        /// <returns></returns>
        [HttpDelete("{ContributorId}", Name = "DeleteContributor")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteContributor(int ContributorId)
        {
            var ContributorInUse = await _repo.ContributionExistsForContributotAsync(ContributorId);
            if (ContributorInUse) { return BadRequest("Contributor cannot be deleted as a contribution has been made by this Contributor"); }

            var ContributorToDelete = await _repo.GetContributorAsync(ContributorId);
            if (ContributorToDelete == null)
            {
                return NotFound($"Contributor with Id = {ContributorId} not found");
            }

            await _repo.DeleteContributor(ContributorId);
            return NoContent();
        }
    }
}