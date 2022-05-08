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
        public async Task balance(IUser user = null)
        {
		    var userInfo = user ?? Context.Client.CurrentUser;
            string Messages = userInfo.Username + "さんの残高は99999アドです。\n\n";

            await ReplyAsync(Messages);
        }
    }
}
