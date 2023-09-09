using API.Authorization;
using API.Data.DTOs;
using API.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiKey]
    [Route("api/devices")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly ISensorRepository _repo;

        private readonly ILogger<SensorController> _logger;

        public SensorController(ISensorRepository repo, ILogger<SensorController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpPost("{deviceId}/telemetry")]
        public async Task<IActionResult> Post(int deviceId, [FromBody] List<AddTelemetryDTO> telemetries)
        {
            if (ModelState.IsValid)
            {
                await _repo.AddDataAsync(deviceId, telemetries);
                _logger.LogInformation("Data added to database");
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet("{deviceId}/statistics")]
        public async Task<IActionResult> Get(int deviceId)
        {
            var stats = await _repo.GetDataAsync(deviceId);
            _logger.LogInformation("Data retrieved from database");
            return Ok(stats);
        }


    }
}
