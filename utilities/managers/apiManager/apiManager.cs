using movieRateGuess.utilities.managers.sqlManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace movieRateGuess.utilities.managers.apiManager
{
    public class apiManager : apiManagerAbstract
    {

        public apiManager(sqlManagerInterface sqlManager) : base(sqlManager)
        {

        }
    }
}
