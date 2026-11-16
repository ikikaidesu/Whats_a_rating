using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace movieRateGuess.models
{
    public class m_menu
    {
        public string checkIfLinkIsValid(string link)
        {
            string result = "";
            if (string.IsNullOrEmpty(link) || link.StartsWith("https://www.kinopoisk.ru/user/") == false)
            {
                return result;
            }
            int indFriendIdStart = 30;
            int indFriendIdEnd = link.IndexOf("/", indFriendIdStart);
            if (indFriendIdEnd != -1)
            {
                result = link.Substring(indFriendIdStart, indFriendIdEnd - indFriendIdStart);
            }

            return result;
        }
    }
}
