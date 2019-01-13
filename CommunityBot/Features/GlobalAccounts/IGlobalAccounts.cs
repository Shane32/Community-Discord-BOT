using System;
using System.Collections.Generic;
using System.Text;

namespace CommunityBot.Features.GlobalAccounts
{
    public interface IGlobalAccounts
    {
        void SaveAccounts(params ulong[] ids);
    }
}
