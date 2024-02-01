using AutoMapper;
using Classes;
using Entities.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using DALEntity = Data.Entities;
using DTO = Data.DTOs;

namespace SampleAPI.Controllers
{
    [Produces("application/json", "application/xml", "text/csv")]
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Funds")]
    [ApiExplorerSettings(GroupName = "2. Funds")]
    public class FundsController : ControllerBase
    {   
        private readonly IFundRepository _repo;       
        private readonly IMapper _mapper;

        public FundsController(IFundRepository repo, IMapper mapper)
        {           
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a list of Funds
        /// </summary>
        /// <returns>An ActionResult of type IEnumerable of Fund</returns>
        [HttpGet(Name = "GetFunds")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Data.Entities.Fund>>> GetFunds()
        {
            var FundsFromRepo = await _repo.GetFundsAsync();
            return Ok(_mapper.Map<List<Data.Entities.Fund>>(FundsFromRepo));
        }

        /// <summary>
        /// Get an Fund by Fund Id
        /// </summary>
        /// <param name="FundId">The id of the Fund you want to get</param>
        /// <returns>An ActionResult of type Fund</returns>
        [HttpGet("{FundId}", Name = "GetFund")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Data.Entities.Fund>> GetFund(int FundId)
        {
            var FundFromRepo = await _repo.GetFundAsync(FundId);
            if (FundFromRepo == null)  { return NotFound(); }
            return Ok(_mapper.Map<Data.Entities.Fund>(FundFromRepo));
        }

        /// <summary>
        /// Adding a new Fund
        /// </summary>
        /// <param name="Fund"></param>
        /// <returns></returns> 
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Fund
        ///     {
        ///        "fundName": "Name of fund",                      
        ///        "comments": "optional comments" 
        ///     }
        ///
        /// </remarks>
        [HttpPost(Name = "CreateFund")]
        [Consumes("application/json")]
        public async Task<ActionResult<Data.Entities.Fund>> CreateFund(DTO.FundForCreation Fund)
        {                      
            var entity = _mapper.Map<DALEntity.Fund>(Fund);
            entity.ProductId = Globals.ApplicationProductId; // Globals.ApplicationProductId was set in AuthorizationController HttpPost
            _repo.AddFund(entity);           
            await _repo.SaveChangesAsync();

            var FundToReturn = _mapper.Map<DTO.Fund>(entity);
            return CreatedAtRoute("GetFund", new { FundId = entity.Id }, FundToReturn);
        }

        /// <summary>
        /// Updates an Fund by Fund id
        /// </summary>
        /// <param name="FundId"></param>
        /// <param name="Fund"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Fund
        ///     {
        ///        "fundName": "description of Fund",
        ///        "comments": "optional comments" 
        ///     }
        ///
        /// </remarks>
        [HttpPut("{FundId}", Name = "UpdateFund")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateFund(int FundId, DTO.FundForCreation Fund)
        {            
            // put updates all fields for an entity so if do not supply values it will reset to default values. 
            var FundFromRepo = await _repo.GetFundAsync(FundId);
            if (FundFromRepo == null) { return NotFound(); }
            _mapper.Map(Fund, FundFromRepo);
            _repo.UpdateFund(FundFromRepo);
            await _repo.SaveChangesAsync();
            return Ok(_mapper.Map<Data.Entities.Fund>(FundFromRepo)); 
        }

        /// <summary>
        /// Deletes an Fund by Fund id
        /// </summary>
        /// <param name="FundId"></param>
        /// <returns></returns>
        [HttpDelete("{FundId}", Name = "DeleteFund")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteFund(int FundId)
        {           
            var FundToDelete = await _repo.GetFundAsync(FundId);
            if (FundToDelete == null)
            {
                throw new FundNotFoundException(FundId);                
            }
            await _repo.DeleteFund(FundId);
            return NoContent();
        }
    }
}