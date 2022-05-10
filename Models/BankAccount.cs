using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VestalisQuintet.EconomyBot.Models
{
    public class BankAccount {
        public int BankAccountId { get; set;}
        public string AccountName { get; set;}
        public User Owner { get; set;}
        public int Balance { get; set;}
    }
}
