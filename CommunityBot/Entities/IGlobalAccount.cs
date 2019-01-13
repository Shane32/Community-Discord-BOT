using System;
using System.Collections.Generic;

namespace CommunityBot.Entities
{
    public interface IGlobalAccount : IEquatable<IGlobalAccount>
    {
        ulong Id { get; }
    }
}
