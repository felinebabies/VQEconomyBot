using Discord;
using VestalisQuintet.EconomyBot.Models;

namespace VestalisQuintet.EconomyBot
{
    public class EconomyLogic 
    {

        /// <summary>
        /// Discordユーザー情報から現在設定中の銀行口座を取得する
        /// </summary>
        /// <param name="db"></param>
        /// <param name="discordUser"></param>
        /// <returns></returns>
        public static BankAccount GetBankAccountFromDiscordUser(VQEconomyBotDbContext db, IUser discordUser)
        {
            BankAccount senderAccount;
            var senderUserQuery = db.Users.Where(item => item.DiscordId == discordUser.Id);
            if (senderUserQuery.Count() == 0)
            {
                // ユーザが存在しない
                // ユーザを作る（自動的に口座もできる）
                var newUser = AccountCreator.CreateUser(db, discordUser.Id, discordUser.Username);
                senderAccount = db.BankAccounts.Where(item => item.BankAccountId == newUser.CurrentBalanceId).First();
            }
            else
            {
                // 残高テーブルを参照する
                var objUser = senderUserQuery.First();
                if (objUser.CurrentBalanceId < 0)
                {
                    // まだ口座を開設していない
                    // 口座を作る
                    var newBankAccount = AccountCreator.CreateBankAccount(db, objUser, "Primary bank account");
                    AccountCreator.SetCurrentBankAccountToUser(db, objUser, newBankAccount);
                    senderAccount = newBankAccount;
                }
                else
                {
                    senderAccount = db.BankAccounts.Where(item => item.BankAccountId == objUser.CurrentBalanceId).First();
                }
            }

            return senderAccount;
        }

        /// <summary>
        /// senderからrecipientの使用中の口座にvalue分の送金を行う
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sender"></param>
        /// <param name="recipient"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SendMoney(VQEconomyBotDbContext db, IUser sender, IUser recipient, int value)
        {
            Models.BankAccount senderAccount;

            senderAccount = EconomyLogic.GetBankAccountFromDiscordUser(db, sender);

            int senderBalance = senderAccount.Balance;
            if (senderBalance < value)
            {
                // 支払い中止
                return(false);
            }
            else
            {
                // 支払元口座からvalueを減算する
                senderAccount.Balance = senderAccount.Balance - value;
                db.BankAccounts.Update(senderAccount);

                // 支払先口座にvalueを加算する
                Models.BankAccount recipientAccount;
                recipientAccount = EconomyLogic.GetBankAccountFromDiscordUser(db, recipient);
                recipientAccount.Balance = recipientAccount.Balance + value;

                db.BankAccounts.Update(recipientAccount);

                // 保存
                db.SaveChanges();
            }

            return(true);
        }
    }
}
