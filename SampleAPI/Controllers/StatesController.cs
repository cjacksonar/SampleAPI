using AutoMapper;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace SampleAPI.Controllers
{
    [Produces("application/json", "application/xml")]
    [ApiController]    
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/States")]
    [ApiExplorerSettings(GroupName = "1. States")]
    public class StatesController : ControllerBase
    {       

        private readonly IStateRepository _repo;     
        private readonly IMapper _mapper;

        public StatesController(IStateRepository repo, IMapper mapper)
        {            
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a list of states
        /// </summary>
        /// <returns>An ActionResult of type IEnumerable of State</returns>
        [HttpGet(Name = "GetStates")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<State>>> GetStates()
        {
            var statesFromRepo = await _repo.GetStatesAsync();
            return Ok(_mapper.Map<IEnumerable<State>>(statesFromRepo));
        }
    }
}