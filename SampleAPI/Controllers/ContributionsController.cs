using AutoMapper;
using Classes;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using DTO = Data.DTOs;

namespace SampleAPI.Controllers
{
    [Produces("application/json", "application/xml")]
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Contributions")]
    [ApiExplorerSettings(GroupName = "4. Contributions")]    
    public class ContributionsController : ControllerBase
    {      
        private readonly IContributionRepository _repo;
        private readonly IContributorRepository _repoContributor;      
        private readonly IMapper _mapper;

        public ContributionsController(IContributionRepository repo, IContributorRepository repoContributor, IMapper mapper)
        {            
            _repo = repo;
            _repoContributor = repoContributor;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a list of years that have contributions
        /// </summary>
        /// <returns>An ActionResult of type List of contribution years</returns>
        [HttpGet("contributionyears", Name = "GetContributionYears")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<string>>> GetContributionYears()
        {
            var contributionYearsFromRepo = await _repo.GetContributionYearsAsync();
            return Ok(contributionYearsFromRepo);
        }

        /// <summary>
        /// Get a list of Contributions Chart Data by Fund and Year
        /// </summary>
        /// <returns>An ActionResult of type List of Contribution Chart Data by Fund and Year</returns>
        [HttpGet("contributionschartdata", Name = "GetContributionsChartData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<DTO.ContributionChartData>>> GetContributionsChartData(int fundId, string selectedYear)
        {
            var ContributionsFromRepo = await _repo.GetContributionChartDataAsync(fundId, selectedYear);
            return Ok(_mapper.Map<List<DTO.ContributionChartData>>(ContributionsFromRepo));
        }

        /// <summary>
        /// Get a total contributions for a selected date
        /// </summary>
        /// <returns>An ActionResult of type List of DailyContributionData</returns>
        [HttpGet("dailycontributionstotal", Name = "GetDailyContributionsTotal")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<DTO.DailyContributionData>>> GetContributionsAnnualReportData(DateTime selectedDate)
        {
            var ContributionsFromRepo = await _repo.GetDailyContributionDataAsync(selectedDate);
            return Ok(_mapper.Map<List<DTO.DailyContributionData>>(ContributionsFromRepo));
        }

        /// <summary>
        /// Get a list of Contributions by Year
        /// </summary>
        /// <returns>An ActionResult of type List of Contribution Chart Data by Fund and Year</returns>
        [HttpGet("contributionsannualreportdata", Name = "GetContributionsAnnualReportData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<DTO.ContributionAnnualReportData>>> GetContributionsAnnualReportData(string selectedYear)
        {
            var ContributionsFromRepo = await _repo.GetContributionAnnualReportDataAsync(selectedYear);
            return Ok(_mapper.Map<List<DTO.ContributionAnnualReportData>>(ContributionsFromRepo));

        }

        /// <summary>
        /// Get a list of Contributions
        /// </summary>
        /// <returns>An ActionResult of type IEnumerable of Contribution</returns>
        [HttpGet(Name = "GetContributions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Contribution>>> GetContributions()
        {
            var ContributionsFromRepo = await _repo.GetContributionsAsync();
            return Ok(_mapper.Map<List<Contribution>>(ContributionsFromRepo));
        }

        /// <summary>
        /// Get a list of Contributions
        /// </summary>
        /// <returns>An ActionResult of type IEnumerable of Contribution</returns>
        [HttpGet("contributionsview", Name = "GetContributionsViewList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Data.DTOs.ViewContributionsList>>> GetContributionsView()
        {
            var ContributionsFromRepo = await _repo.GetContributionsViewListAsync();
            return Ok(_mapper.Map<List<Data.DTOs.ViewContributionsList>>(ContributionsFromRepo));
        }

        /// <summary>
        /// Get an Contributions by Contributor Id
        /// </summary>
        /// <param name="contributorId">The id of the Contributor you want to get</param>
        /// <returns>An ActionResult of type Contribution</returns>
        [HttpGet("{contributorId}/contributions", Name = "GetContributionsByContributor")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Contribution>> GetContributionByContributor(int contributorId)
        {
            var ContributorFromRepo = await _repoContributor.GetContributorAsync(contributorId);
            if (ContributorFromRepo == null) { return NotFound("Contributor was not found"); }

            var ContributionsFromRepo = await _repo.GetContributionByContributorAsync(contributorId);
            if (ContributionsFromRepo == null) { return NotFound("There are now contributions for this contributor"); }
            return Ok(_mapper.Map<List<Contribution>>(ContributionsFromRepo));
        }

        /// <summary>
        /// Get an Contribution by Contribution Id
        /// </summary>
        /// <param name="ContributionId">The id of the Contribution you want to get</param>
        /// <returns>An ActionResult of type Contribution</returns>
        [HttpGet("{ContributionId}", Name = "GetContribution")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Contribution>> GetContribution(int ContributionId)
        {
            var ContributionFromRepo = await _repo.GetContributionAsync(ContributionId);
            if (ContributionFromRepo == null) { return NotFound(); }
            return Ok(_mapper.Map<Contribution>(ContributionFromRepo));
        }

        /// <summary>
        /// Adding a new Contribution
        /// </summary>
        /// <param name="Contribution"></param>
        /// <returns></returns> 
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Contribution
        ///     {
        ///       "contributionDate": "4/21/2023",     
        ///       "amount": 10,     
        ///       "checkNumber": 0,
        ///       "comments": "string",
        ///       "fundId": 2,                               
        ///       "contributorId": 2
        ///     }
        ///
        /// </remarks>
        [HttpPost(Name = "CreateContribution")]
        [Consumes("application/json")]
        public async Task<ActionResult<Data.Entities.Contribution>> CreateContribution(DTO.ContributionForCreation Contribution)
        {
            var entity = _mapper.Map<Data.Entities.Contribution>(Contribution);
            entity.ProductId = Globals.ApplicationProductId; // this was set in authorize in swagger or in request header
            _repo.AddContribution(entity);
            await _repo.SaveChangesAsync();

            var ContributionToReturn = _mapper.Map<DTO.Contribution>(entity);
            return CreatedAtRoute("GetContribution", new { ContributionId = entity.Id }, ContributionToReturn);
        }

        /// <summary>
        /// Updates an Contribution by Contribution id
        /// </summary>
        /// <param name="ContributionId"></param>
        /// <param name="Contribution"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Contribution
        ///     {
        ///       "contributionDate": "4/21/2023",     
        ///       "amount": 20,     
        ///       "checkNumber": 0,
        ///       "comments": "string",
        ///       "fundId": 2,                               
        ///       "contributorId": 2
        ///     }
        ///
        /// </remarks>
        [HttpPut("{ContributionId}", Name = "UpdateContribution")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateContribution(int ContributionId, DTO.ContributionForCreation Contribution)
        {
            // put updates all fields for an entity so if do not supply values it will reset to default values. In this case works, only one field to update.
            var ContributionFromRepo = await _repo.GetContributionAsync(ContributionId);
            if (ContributionFromRepo == null) { return NotFound(); }
            _mapper.Map(Contribution, ContributionFromRepo);
            _repo.UpdateContribution(ContributionFromRepo);
            await _repo.SaveChangesAsync();
            return Ok(_mapper.Map<Contribution>(ContributionFromRepo)); // return the Contribution            
        }

        /// <summary>
        /// Deletes an Contribution by Contribution id
        /// </summary>
        /// <param name="ContributionId"></param>
        /// <returns></returns>
        [HttpDelete("{ContributionId}", Name = "DeleteContribution")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteContribution(int ContributionId)
        {
            var ContributionToDelete = await _repo.GetContributionAsync(ContributionId);
            if (ContributionToDelete == null)
            {
                return NotFound($"Contribution with Id = {ContributionId} not found");
            }
            await _repo.DeleteContribution(ContributionId);
            return NoContent();
        }
    }
}