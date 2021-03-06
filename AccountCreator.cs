

using VestalisQuintet.EconomyBot.Models;

namespace VestalisQuintet.EconomyBot
{
    public class AccountCreator 
    {
        /// <summary>
        /// 新規ユーザを作って追加する
        /// また、自動的に標準の口座も作られる
        /// </summary>
        /// <param name="db"></param>
        /// <param name="discordId"></param>
        /// <param name="nickName"></param>
        /// <returns></returns>
        public static User CreateUser(VQEconomyBotDbContext db, ulong discordId, string nickName)
        {
            User newUser = new User();
            newUser.DiscordId = discordId;
            newUser.NickName = nickName;
            newUser.CurrentBalanceId = -1;

            db.Users.Add(newUser);
            db.SaveChanges();

            var newBankAccount = CreateBankAccount(db, newUser, "Primary bank account");
            SetCurrentBankAccountToUser(db, newUser, newBankAccount);

            return (newUser);
        }

        /// <summary>
        /// ユーザーに現在使用中の銀行口座を設定する
        /// </summary>
        /// <param name="db"></param>
        /// <param name="newUser"></param>
        /// <param name="newBankAccount"></param>
        public static void SetCurrentBankAccountToUser(VQEconomyBotDbContext db, User newUser, BankAccount newBankAccount)
        {
            newUser.CurrentBalanceId = newBankAccount.BankAccountId;

            db.Users.Update(newUser);
            db.SaveChanges();
        }

        /// <summary>
        /// ユーザーに新しく銀行口座を作成する
        /// </summary>
        /// <param name="db"></param>
        /// <param name="owner"></param>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public static BankAccount CreateBankAccount(VQEconomyBotDbContext db, User owner, string accountName)
        {
            BankAccount newAccount = new BankAccount();
            newAccount.Balance = 0;
            newAccount.AccountName = accountName;
            newAccount.Owner = owner;

            db.BankAccounts.Add(newAccount);
            db.SaveChanges();

            return(newAccount); 
        }
    }

}
