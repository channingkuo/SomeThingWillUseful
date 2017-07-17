using System;
using System.Collections.Generic;

namespace RekTec.Chat.Views.Chat
{
    public static class EmojiUtil
    {
        public static string[] Chars = {
            "[龇牙]", "[哭笑]", "[微笑]", "[微笑]", "[汗笑]", "[微笑]", "[眨眼]", "[羞笑]",
            "[品尝]", "[轻松]", "[爱你]", "[假笑]", "[不开心]", "[冷汗]", "[沉思]", "[困惑]",
            "[飞吻]", "[亲吻]", "[鬼脸]", "[鬼脸]", "[失望]", "[生气]", "[噘嘴]", "[哭]",

            "[坚持]", "[欢欣]", "[失望]", "[害怕]", "[疲倦]", "[昏昏]", "[疲倦]", "[大哭]",
            "[冷汗]", "[恐惧]", "[晕眩]", "[激动]", "[晕]", "[生病]"
        };

        public static string[] Unicodes = {
            "\U0001F601", "\U0001F602", "\U0001F603", "\U0001F604", "\U0001F605", "\U0001F606", "\U0001F609", "\U0001F60A",
            "\U0001F60B", "\U0001F60C", "\U0001F60D", "\U0001F60F", "\U0001F612", "\U0001F613", "\U0001F614", "\U0001F616",
            "\U0001F618", "\U0001F61A", "\U0001F61C", "\U0001F61D", "\U0001F61E", "\U0001F620", "\U0001F621", "\U0001F622",

            "\U0001F623", "\U0001F624", "\U0001F625", "\U0001F628", "\U0001F629", "\U0001F62A", "\U0001F62B", "\U0001F62D",
            "\U0001F630", "\U0001F631", "\U0001F632", "\U0001F633", "\U0001F635", "\U0001F637",
        };

        public static int Count = 38;

        public static string ReplaceCharWithUnicode(string msg)
        {
            for (var i = 0; i < Count; i++) {
                msg = msg.Replace(Chars[i], Unicodes[i]);
            }

            return msg;
        }
    }
}

