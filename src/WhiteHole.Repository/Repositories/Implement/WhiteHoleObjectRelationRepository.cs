using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteHole.Repository.Repositories
{
    public class WhiteHoleObjectRelationRepository : RepositoryBase<Models.WhiteHoleObjectRelation,
        RepositoryContext>, IWhiteHoleObjectRelationRepository
    {
        public WhiteHoleObjectRelationRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }
    }
}
