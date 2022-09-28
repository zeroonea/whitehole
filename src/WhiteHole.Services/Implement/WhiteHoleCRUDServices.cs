using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Cryptography;
using WhiteHole.DTO;
using WhiteHole.Repository.Models;
using WhiteHole.Repository.Repositories;

namespace WhiteHole.Services
{
    public class WhiteHoleCRUDServices : IWhiteHoleCRUDServices
    {
        private readonly ILogger<WhiteHoleCRUDServices> _logger;
        private readonly IWhiteHoleObjectKVRepository _objectKVRepo;
        private readonly IWhiteHoleObjectRepository _objectRepo;
        private readonly IWhiteHoleObjectRelationRepository _objectRepoRel;

        public WhiteHoleCRUDServices(ILogger<WhiteHoleCRUDServices> logger,
            IWhiteHoleObjectKVRepository objectKVRepo,
            IWhiteHoleObjectRepository objectRepo,
            IWhiteHoleObjectRelationRepository objectRepoRel)
        {
            _logger = logger;
            _objectRepo = objectRepo;
            _objectKVRepo = objectKVRepo;
            _objectRepoRel = objectRepoRel;
        }

        public async Task<WhiteHoleObjectCreateResponse> Create(string path, string json)
        {
            var res = new WhiteHoleObjectCreateResponse();
            var pathRes = Util.PathParser(path);
            if (pathRes != null && pathRes[Constants.PATH_LAST_KEY] == Constants.PATH_LAST_OBJ)
            {
                WhiteHoleObject prevObj = null;
                for (var i = 1; ; i++)
                {
                    if (!pathRes.ContainsKey($"obj_{i}")) break;
                    var objName = pathRes[$"obj_{i}"];

                    if (!pathRes.ContainsKey($"obj_id_{i}"))
                    {
                        var obj = CreateObject(objName, json);
                        if (prevObj == null)
                        {
                            await _objectRepo.SaveAsync();
                            res.message = "success";
                            res.created = new Dictionary<string, List<Dictionary<string, object>>>
                            {
                                [objName] = new List<Dictionary<string, object>>{
                                    Util.Json2Object(obj.Id, json)
                                }
                            };
                            return res;
                        }
                        else
                        {
                            var obj1nRel = new WhiteHoleObjectRelation
                            {
                                Object1 = prevObj,
                                Object1Name = prevObj.ObjectName,
                                ObjectN = obj,
                                ObjectNName = obj.ObjectName
                            };
                            Util.StampFingerPrint(obj1nRel);
                            _objectRepoRel.Add(obj1nRel);
                            await _objectRepoRel.SaveAsync();
                            res.message = "success";
                            res.created = new Dictionary<string, List<Dictionary<string, object>>>
                            {
                                [objName] = new List<Dictionary<string, object>>{
                                    Util.Json2Object(obj.Id, json)
                                }
                            };
                            return res;
                        }
                    }
                    else
                    {
                        var objId = int.Parse(pathRes[$"obj_id_{i}"]);
                        prevObj = await _objectRepo.FindByCondition(p => p.Id == objId && p.ObjectName == objName)
                            .AsTracking()
                            .FirstOrDefaultAsync();
                        if (prevObj == null)
                        {
                            res.message = $"Error: {objName}#{objId} not found";
                            return res;
                        }
                    }
                }
            }

            res.message = "unknown";
            return res;
        }

        public async Task<WhiteHoleObjectUpdateResponse> Put(string path, string json)
        {
            var res = new WhiteHoleObjectUpdateResponse();
            var obj = await GetObjectForUpdate(path);
            if (obj == null)
            {
                res.message = "Error: not found";
                return res;
            }

            obj.ObjectJson = json;
            Util.StampFingerPrint(obj, true);
            _objectRepo.Update(obj);

            var objNewData = Util.Json2Object(json);
            var kvs = await _objectKVRepo.FindByCondition(p => p.ObjectName == obj.ObjectName && p.ObjectId == obj.Id)
                .AsTracking().ToListAsync();
            // Update or Delete existing KV
            foreach (var kv in kvs)
            {
                if (!objNewData.ContainsKey(kv.ObjectKey))
                {
                    _objectKVRepo.Delete(kv);
                }
                else
                {
                    kv.ObjectValue = objNewData[kv.ObjectKey].ToString();
                    Util.StampFingerPrint(kv, true);
                    _objectKVRepo.Update(kv);
                }
            }
            // Add new KV
            foreach (var k in objNewData.Keys)
            {
                if (!kvs.Any(p => p.ObjectKey == k))
                {
                    var kvm = new WhiteHoleObjectKV
                    {
                        Object = obj,
                        ObjectName = obj.ObjectName,
                        ObjectKey = k,
                        ObjectValue = objNewData[k].ToString()
                    };
                    Util.StampFingerPrint(kvm);
                    _objectKVRepo.Add(kvm);
                }
            }
            // Compose response
            objNewData["id"] = obj.Id;
            res.message = "success";
            res.updated = new Dictionary<string, List<Dictionary<string, object>>>
            {
                [obj.ObjectName] = new List<Dictionary<string, object>>
                {
                    objNewData
                }
            };
            await _objectKVRepo.SaveAsync();

            return res;
        }

        public async Task<WhiteHoleObjectUpdateResponse> Patch(string path, string json)
        {
            var res = new WhiteHoleObjectUpdateResponse();
            var obj = await GetObjectForUpdate(path);
            if (obj == null)
            {
                res.message = "Error: not found";
                return res;
            }
            var objNewData = Util.Json2Object(json);
            var objOldData = Util.Json2Object(obj.ObjectJson);
            var kvs = await _objectKVRepo.FindByCondition(p => p.ObjectName == obj.ObjectName && p.ObjectId == obj.Id)
                .AsTracking().ToListAsync();

            // Update existing KV
            foreach (var kv in kvs)
            {
                if (objNewData.ContainsKey(kv.ObjectKey))
                {
                    kv.ObjectValue = objNewData[kv.ObjectKey].ToString();
                    objOldData[kv.ObjectKey] = kv.ObjectValue;
                    Util.StampFingerPrint(kv, true);
                    _objectKVRepo.Update(kv);
                }
            }
            // Add new KV
            foreach (var k in objNewData.Keys)
            {
                if (!kvs.Any(p => p.ObjectKey == k))
                {
                    var kvm = new WhiteHoleObjectKV
                    {
                        Object = obj,
                        ObjectName = obj.ObjectName,
                        ObjectKey = k,
                        ObjectValue = objNewData[k].ToString()
                    };
                    Util.StampFingerPrint(kvm);
                    _objectKVRepo.Add(kvm);
                    objOldData[k] = kvm.ObjectValue;
                }
            }
            obj.ObjectJson = JsonConvert.SerializeObject(objOldData);
            Util.StampFingerPrint(obj, true);
            _objectRepo.Update(obj);

            // Compose response
            objOldData["id"] = obj.Id;
            res.message = "success";
            res.updated = new Dictionary<string, List<Dictionary<string, object>>>
            {
                [obj.ObjectName] = new List<Dictionary<string, object>>
                {
                    objOldData
                }
            };
            await _objectKVRepo.SaveAsync();
            return res;
        }

        private async Task<WhiteHoleObject> GetObjectForUpdate(string path)
        {
            var pathRes = Util.PathParser(path);
            if (pathRes != null && pathRes[Constants.PATH_LAST_KEY] == Constants.PATH_LAST_ID
                && pathRes[Constants.PATH_OBJ_COUNT_KEY] == "1")
            {
                var objName = pathRes["obj_1"];
                var objId = int.Parse(pathRes["obj_id_1"]);
                return await _objectRepo.FindByCondition(p => p.Id == objId && p.ObjectName == objName)
                    .AsTracking()
                    .FirstOrDefaultAsync();
            }

            return null;
        }


        private WhiteHoleObject CreateObject(string name, string json)
        {
            var obj = new WhiteHoleObject
            {
                ObjectName = name,
                ObjectJson = json
            };
            Util.StampFingerPrint(obj);
            _objectRepo.Add(obj);

            var kvs = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            foreach (var k in kvs.Keys)
            {
                var kvm = new WhiteHoleObjectKV
                {
                    Object = obj,
                    ObjectName = name,
                    ObjectKey = k,
                    ObjectValue = kvs[k]
                };
                Util.StampFingerPrint(kvm);
                _objectKVRepo.Add(kvm);
            }

            return obj;
        }
    }
}