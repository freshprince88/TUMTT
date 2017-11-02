using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Models.Api;

namespace TT.Lib.Managers
{
    public interface IMatchLibraryManager
    {
        IEnumerable<MatchMeta> GetMatches(String query=null);
        MatchMeta FindMatch(Guid guid);
    }
}
