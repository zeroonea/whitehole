using Microsoft.Extensions.Logging;
using WhiteHole.DTO;
using WhiteHole.Repository.Repositories;

namespace WhiteHole.Services
{
    public interface IWhiteHoleCRUDServices
    {
        public Task<WhiteHoleObjectCreateResponse> Create(string path, string json);
        public Task<WhiteHoleObjectUpdateResponse> Put(string path, string json);
        public Task<WhiteHoleObjectUpdateResponse> Patch(string path, string json);
    }
}