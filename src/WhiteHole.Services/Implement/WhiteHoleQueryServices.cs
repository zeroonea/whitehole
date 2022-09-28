using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;
using System.Text.Json.Nodes;
using WhiteHole.DTO;
using WhiteHole.Repository.Models;
using WhiteHole.Repository.Repositories;

namespace WhiteHole.Services
{
    public class WhiteHoleQueryServices : IWhiteHoleQueryServices
    {
        private readonly ILogger<WhiteHoleQueryServices> _logger;
        private readonly IWhiteHoleObjectKVRepository _objectKVRepo;
        private readonly IWhiteHoleObjectRepository _objectRepo;
        private readonly IWhiteHoleObjectRelationRepository _objectRepoRel;

        public WhiteHoleQueryServices(ILogger<WhiteHoleQueryServices> logger,
            IWhiteHoleObjectKVRepository objectKVRepo,
            IWhiteHoleObjectRepository objectRepo,
            IWhiteHoleObjectRelationRepository objectRepoRel)
        {
            _logger = logger;
            _objectRepo = objectRepo;
            _objectKVRepo = objectKVRepo;
            _objectRepoRel = objectRepoRel;
        }

        public async Task<Dictionary<string, object>> Get(Dictionary<string, string> pathRes)
        {
            return await GetObject(pathRes["obj_1"], int.Parse(pathRes["obj_id_1"]));
        }

        public async Task<List<Dictionary<string, object>>> GetList(Dictionary<string, string> pathRes, string sort)
        {
            if (pathRes != null && pathRes[Constants.PATH_LAST_KEY] == Constants.PATH_LAST_OBJ 
                && (pathRes[Constants.PATH_OBJ_COUNT_KEY] == "1" || pathRes[Constants.PATH_OBJ_COUNT_KEY] == "2"))
            {
                IQueryable<WhiteHoleObjectRelation> querysetrv = _objectRepoRel.All();
                var lastObjName = "";
                for (var i = 1; ; i++)
                {
                    if (!pathRes.ContainsKey($"obj_{i}")) break;
                    var objName = pathRes[$"obj_{i}"];
                    if (!pathRes.ContainsKey($"obj_id_{i}"))
                    {
                        if(string.IsNullOrEmpty(lastObjName))
                        {
                            return await GetObjects(objName, sort);
                        }
                        else
                        { 
                            querysetrv = querysetrv.Where(p => p.ObjectNName == objName);
                        }
                    }
                    else
                    {
                        var objId = int.Parse(pathRes[$"obj_id_{i}"]);
                        querysetrv = querysetrv.Where(p => p.Object1Id == objId && p.Object1Name == objName);
                    }
                    lastObjName = objName;
                }

                if(querysetrv != null)
                {
                    if (!string.IsNullOrEmpty(sort))
                    {
                        return await Sorting(_objectRepo.All(), 
                            sort, querysetrv);
                    }

                    return await _objectRepo.FindByCondition(p => querysetrv.Select(p => p.ObjectNId).Contains(p.Id) && p.ObjectName == lastObjName)
                            .Select(p => Util.Json2Object(p.Id, p.ObjectJson))
                            .ToListAsync();
                }
            }
            return new List<Dictionary<string, object>>();
        }

        private async Task<Dictionary<string, object>> GetObject(string name, int id)
        {
            var obj = await _objectRepo.FindByCondition(p => p.Id == id && p.ObjectName == name)
                .FirstOrDefaultAsync();
            
            if (obj != null)
            {
                return Util.Json2Object(id, obj.ObjectJson);
            }

            return null;
        }

        private async Task<List<Dictionary<string, object>>> GetObjects(string name, string sort = null)
        {
            if (string.IsNullOrEmpty(sort))
            {
                return await _objectRepo.FindByCondition(p => p.ObjectName == name).Select(p => Util.Json2Object(p.Id, p.ObjectJson)).ToListAsync();
            }
            return await Sorting(_objectRepo.FindByCondition(p => p.ObjectName == name), sort);
        }

        private async Task<List<Dictionary<string, object>>> Sorting(IQueryable<WhiteHoleObject> queryset, string sort, 
            IQueryable<WhiteHoleObjectRelation> querysetrv = null)
        {
            var isDesc = sort.StartsWith("-");
            sort = isDesc ? sort.Substring(1) : sort;
            var sorting = (from a in _objectKVRepo.All()
                where a.ObjectKey == sort
                    && (querysetrv == null || querysetrv.Select(p => p.ObjectNId).Contains(a.ObjectId))
                select a);

            if (isDesc)
            {
                sorting = sorting.OrderByDescending(p => p.ObjectValue);
            }
            else
            {
                sorting = sorting.OrderBy(p => p.ObjectValue);
            }

            return await (from a in sorting
                            join b in queryset on a.ObjectId equals b.Id
                            select Util.Json2Object(b.Id, b.ObjectJson)).ToListAsync();
        }
    }
}