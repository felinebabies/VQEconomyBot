using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;

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
    }
}
