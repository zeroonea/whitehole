using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteHole.Repository;

namespace WhiteHole.Services
{
    public static class Util
    {
        public static Dictionary<string, string> PathParser(string path)
        {
            var paths = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (paths[0] == Constants.PATH_PREFIX)
            {
                var res = new Dictionary<string, string>();
                var count = 1;
                for (var i = 1; i < paths.Length; i++)
                {
                    if (i % 2 != 0)
                    {
                        res[$"obj_{count}"] = paths[i];
                        count += 2;
                        res[Constants.PATH_LAST_KEY] = Constants.PATH_LAST_OBJ;
                        res[Constants.PATH_OBJ_COUNT_KEY] = $"{count - 2}";
                    }
                    else
                    {
                        res[$"obj_id_{count - 2}"] = paths[i];
                        count--;
                        res[Constants.PATH_LAST_KEY] = Constants.PATH_LAST_ID;
                    }
                }
                return res;
            }

            return null;
        }

        public static Dictionary<string, object> Json2Object(int objId, string json)
        {
            var res = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            res["id"] = objId;
            return res;
        }

        public static Dictionary<string, object> Json2Object(string json)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }

        public static T Get<T>(this Dictionary<string, object> instance, string name)
        {
            return (T)instance[name];
        }

        public static void StampFingerPrint(IModel model, bool isUpdate = false)
        {
            if(!isUpdate)
            { 
                model.CreatedAt = DateTime.UtcNow;
                model.CreatedBy = "guest";
            }
            model.UpdatedAt = DateTime.UtcNow;
            model.UpdatedBy = "guest";
        }
    }
}
