using System.Threading.Tasks;
using JccPropertyHub.Domain.Core.Dto;
using JccPropertyHub.Domain.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JccPropertyHub.WebApi.Controllers {
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PropertiesController : Controller {
        private readonly IJccHubPropertiesService propertiesService;

        public PropertiesController(IJccHubPropertiesService propertiesService) {
            this.propertiesService = propertiesService;
        }

        //// GET: api/values
        //[HttpGet]
        //public async Task<SearchAvailabilityRS> GetAvailablity(SearchAvailabilityRQ request) {
        //    return await propertiesService.SearchAvailability(request);
        //}

        // GET: api/values
        [HttpPost]
        public async Task<SearchAvailabilityRs> GetAvailablity([FromBody] SearchAvailabilityRq request) {
            return await propertiesService.SearchAvailability(request);
        }

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id) {
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody] string value) {
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value) {
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id) {
        //}
    }
}