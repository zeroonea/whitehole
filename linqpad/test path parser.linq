<Query Kind="Program" />

void Main()
{
	var res = PathParser("whitehole/customers/1/");
	res.Dump();
}

public static Dictionary<string, string> PathParser(string path)
{
	var paths = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
	if (paths[0] == "whitehole")
	{
		var res = new Dictionary<string, string>();
		var count = 1;
		for (var i = 1; i < paths.Length; i++)
                {
            if (i % 2 != 0)
            {
				res[$"obj_{count}"] = paths[i];
				count += 2;
				res["last"] = "obj";
				res["count"] = $"{count - 2}";
			}
			else
			{
				res[$"obj_id_{count - 2}"] = paths[i];
				count--;
				res["last"] = "id";
			}
		}
		return res;
	}

	return null;
}
