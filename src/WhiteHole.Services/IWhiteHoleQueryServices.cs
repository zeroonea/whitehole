using Microsoft.Extensions.Logging;
using WhiteHole.DTO;
using WhiteHole.Repository.Repositories;

namespace WhiteHole.Services
{
    public interface IWhiteHoleQueryServices
    {
        public Task<Dictionary<string, object>> Get(Dictionary<string, string> pathRes);
        public Task<List<Dictionary<string, object>>> GetList(Dictionary<string, string> pathRes, string sort);
    }
}