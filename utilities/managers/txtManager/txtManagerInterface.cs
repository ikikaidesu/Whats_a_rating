using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace movieRateGuess.utilities.managers.txtManager
{
    public interface txtManagerInterface
    {
        bool checkIfFileExists();
        string getFileContent();
    }
}
