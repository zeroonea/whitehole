using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteHole.Repository.Repositories
{
    public class WhiteHoleObjectRepository : RepositoryBase<Models.WhiteHoleObject,
        RepositoryContext>, IWhiteHoleObjectRepository
    {
        public WhiteHoleObjectRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }
    }
}
