using API.Data.DTOs;

namespace API.Data.Repository
{
    public interface ISensorRepository
    {
        Task AddDataAsync(int deviceId, List<AddTelemetryDTO> data);
        Task<IEnumerable<TelemetryDTO>> GetDataAsync(int deviceId);

    }
}
