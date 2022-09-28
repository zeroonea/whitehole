<Query Kind="Statements">
  <Connection>
    <ID>242768b1-0aa7-4778-a4ea-281e9341791e</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>222.253.41.239</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>WhiteHole</Database>
    <SqlSecurity>true</SqlSecurity>
    <UserName>ss-admin</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAYNslTaIgi0W0paqwlTxnbgAAAAACAAAAAAAQZgAAAAEAACAAAABI9h2Q/Z1keWwsZv+dSt5ij2C7hgzfk0RsZAM3E0HBogAAAAAOgAAAAAIAACAAAAAUkllGX2gzlMBDUPO6QBGx6s6a1hyrnNyUyavRJoCiixAAAAB4a0GBAMpbc0TJ+k2fBqvcQAAAAP2IZOVHu/zZHtAwdOZdP8v+UkIQO1Lt3xt6SjOoh/TZGaHl9bmLNiUBy4WmyOZ+kbX85LFNocEEDEW6gJASsB4=</Password>
  </Connection>
</Query>

var querysetc = (from c in WhiteHoleObjectRelations
where c.Object1Name == "customers" && c.Object1Id == 9 && c.ObjectNName == "vehicles"
select c.ObjectNId);
//querysetc.Dump();

var queryseta_ = (from a in WhiteHoleKVs
where a.ObjectKey == "rego" && querysetc.Contains(a.ObjectId)
orderby a.ObjectValue
select a);
//var queryseta = queryseta_.OrderByDescending(p => p.ObjectValue).Select(p => p.ObjectId);
//queryseta.Dump();

//var seta = queryseta.ToList();
//seta.Dump();

var setb = (from b in WhiteHoleObjects 
select b);
//).ToList();
//
//setb.Dump();
//
//var setbn = new List<WhiteHoleObject>();
//foreach(var id in seta){
//	//id.Dump();
//	setbn.Add(setb.FirstOrDefault(p => p.Id == id));
//}
//
//setbn.Dump();


(from d in queryseta_ 
join e in setb on d.ObjectId equals e.Id
select e).Dump();