using CommunityBot.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommunityBot.Features.GlobalAccounts
{
    public interface IGlobalUserAccounts : IGlobalAccounts
    {
        GlobalUserAccount GetUserAccount(ulong userId);
    }
}
