using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VestalisQuintet.EconomyBot.Models
{
    public class User {
        public ulong UserId { get; set;}
        public string NickName { get; set;}
        public ulong? DiscordId { get; set;}
        public int CurrentBalanceId { get; set;}
    }
}
