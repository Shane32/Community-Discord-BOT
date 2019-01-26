﻿using CommunityBot.Configuration;
using CommunityBot.Entities;
using Discord;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CommunityBot.Features.GlobalAccounts
{
    public class GlobalGuildAccounts : IGlobalAccounts
    {
        private readonly ConcurrentDictionary<ulong, GlobalGuildAccount> serverAccounts = new ConcurrentDictionary<ulong, GlobalGuildAccount>();
        private readonly JsonDataStorage _jsonDataStorage;

        public GlobalGuildAccounts(JsonDataStorage jsonDataStorage)
        {
            _jsonDataStorage = jsonDataStorage;
            var info = System.IO.Directory.CreateDirectory(Path.Combine(Constants.ResourceFolder,Constants.ServerAccountsFolder));
            var files = info.GetFiles("*.json");
            if (files.Length > 0)
            {
                foreach (var file in files)
                {
                    var server = jsonDataStorage.RestoreObject<GlobalGuildAccount>(Path.Combine(file.Directory.Name, file.Name));
                    serverAccounts.TryAdd(server.Id, server);
                }
            }
        }

        public IEnumerable<KeyValuePair<ulong, GlobalGuildAccount>> GetAll()
        {
            return serverAccounts.ToArray();
        }

        public GlobalGuildAccount GetGuildAccount(ulong id)
        {
            return serverAccounts.GetOrAdd(id, (key) =>
            {
                var newAccount = new GlobalGuildAccount(id);
                _jsonDataStorage.StoreObject(newAccount, Path.Combine(Constants.ServerAccountsFolder, $"{id}.json"), useIndentations: true);
                return newAccount;
            });
        }

        public GlobalGuildAccount GetGuildAccount(IGuild guild)
        {
            return GetGuildAccount(guild.Id);
        }

        /// <summary>
        /// This rewrites ALL ServerAccounts to the harddrive... Strongly recommend to use SaveAccounts(id1, id2, id3...) where possible instead
        /// </summary>
        public void SaveAccounts()
        {
            SaveAccounts(serverAccounts.Keys.ToArray());
        }

        /// <summary>
        /// Saves one or multiple Accounts by provided Ids
        /// </summary>
        public void SaveAccounts(params ulong[] ids)
        {
            foreach (var id in ids)
            {
                _jsonDataStorage.StoreObject(GetGuildAccount(id), Path.Combine(Constants.ServerAccountsFolder, $"{id}.json"), useIndentations: true);
            }
        }
    }
}
