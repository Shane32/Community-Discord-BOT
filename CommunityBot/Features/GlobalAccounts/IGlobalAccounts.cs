using System;
using System.Collections.Generic;
using System.Text;

namespace CommunityBot.Features.GlobalAccounts
{
    interface IGlobalAccounts
    {
        void SaveAccounts(params ulong[] ids);
    }
}
