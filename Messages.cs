using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using VestalisQuintet.EconomyBot.Models;

namespace VestalisQuintet.EconomyBot
{
    public class Messages : ModuleBase
    {
        /// <summary>
        /// データベースコンテキスト
        /// </summary>
        private readonly VQEconomyBotDbContext _database;

        public Messages(VQEconomyBotDbContext database){
            _database = database;
        }

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
            
            string bankAccountName = "Primary bank account";
            int balance = 0;
            // データベースから残高を検索
            var targetAccount = EconomyLogic.GetBankAccountFromDiscordUser(_database, userInfo);

            balance = targetAccount.Balance;
            bankAccountName = targetAccount.AccountName;
            string Messages = userInfo.Username + "さんの口座[" + bankAccountName + "]の残高は" + balance + "アドです。\n\n";

            await ReplyAsync(Messages);
        }

        /// <summary>
        /// 指定のuserに、指定額valueを支払う。
        /// </summary>
        /// <param name="recipient"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [Command("pay")]
        public async Task pay(IUser recipient, int value)
        {
            string Messages = "";
            var sender = Context.Message.Author;

            if(sender.Id == recipient.Id){
                // 送受信者が同一であれば送金中止
                Messages = "送受信者が同一の為、送金を中止しました。\n\n";
            }
            else {
                if(value <= 0){
                    Messages = "支払額は1以上である必要があります。\n\n";
                }
                else {
                    using(var tran = _database.Database.BeginTransaction())
                    {
                        // 支払元口座にvalue以上の金額があることを確認する
                        bool sendResult = EconomyLogic.SendMoney(_database, sender, recipient, value);
                        if(sendResult != true){
                            Messages = "支払い側の口座残高が不足していたため、送金できませんでした。\n\n";
                        }
                        else {
                            Messages = "支払者" + sender.Username + "が受取者" + recipient.Username + "宛に" + value + "アド送金しました。\n\n";
                        }

                        tran.Commit();
                    }
                }
            }

            await ReplyAsync(Messages);
        }
    }
}
