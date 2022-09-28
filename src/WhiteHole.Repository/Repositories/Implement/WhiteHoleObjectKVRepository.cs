using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteHole.Repository.Repositories
{
    public class WhiteHoleObjectKVRepository : RepositoryBase<Models.WhiteHoleObjectKV,
        RepositoryContext>, IWhiteHoleObjectKVRepository
    {
        public WhiteHoleObjectKVRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }
    }
}
