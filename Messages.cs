using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace VestalisQuintet.EconomyBot
{
    public class Messages : ModuleBase
    {
        /// <summary>
        /// [hellovq]というコメントが来た際の処理
        /// </summary>
        /// <returns>Botのコメント</returns>
        [Command("hellovq")]
        public async Task helloVQ()
        {
            string Messages = "めたんもそう思います\n\n";

            await ReplyAsync(Messages);
        }

        /// <summary>
        /// 残高を確認する
        /// </summary>
        /// <returns></returns>
        [Command("balance")]
        public async Task balance(IUser? user = null)
        {
            // 引数なしの場合はコマンド送信者の口座を確認する
		    var userInfo = user ?? Context.Message.Author;
            
            int balance = 0;
            // データベースから残高を検索
            using(var db = new VQEconomyBotDbContext()){
                // まずユーザを検索する
                var userItemQuery = db.Users.Where(userItem => (userItem.DiscordId == userInfo.Id));
                if(userItemQuery.Count() == 0){
                    // ユーザが存在しない
                    balance = 0;

                    // ユーザを作る
                    var newUser = AccountCreator.CreateUser(db, userInfo.Id, userInfo.Username);
                }
                else {
                    // 残高テーブルを参照する
                    var objUser = userItemQuery.First();
                    if(objUser.CurrentBalanceId < 0){
                        // まだ口座を開設していない
                        balance = 0;
                    }
                    else {
                        var balanceQuery = db.BankAccounts.Where(item => item.BankAccountId == objUser.CurrentBalanceId);
                        balance = balanceQuery.First().Balance;
                    }
                }
            }
            string Messages = userInfo.Username + "さんの残高は" + balance + "アドです。\n\n";

            await ReplyAsync(Messages);
        }
    }
}
