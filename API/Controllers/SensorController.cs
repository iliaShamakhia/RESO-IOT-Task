using API.Data.DTOs;
using API.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/devices")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly ISensorRepository _repo;

        public SensorController(ISensorRepository repo)
        {
               _repo = repo;
        }

        [HttpPost("{deviceId}/telemetry")]
        public async Task<IActionResult> Post(int deviceId, [FromBody] List<AddTelemetryDTO> telemetries)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _repo.AddDataAsync(deviceId, telemetries);
                    return Ok();
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{deviceId}/statistics")]
        public async Task<IActionResult> Get(int deviceId)
        {
            var stats = await _repo.GetDataAsync(deviceId);
            return Ok(stats);
        }


    }
}
