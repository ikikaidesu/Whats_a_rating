using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Resources;

namespace movieRateGuess.utilities.managers.txtManager
{
    public abstract class txtManagerAbstract : txtManagerInterface
    {
        private readonly string _path = "pack://application:,,,/movieRateGuess;component/resources/data/apiKey.txt";
        public bool checkIfFileExists()
        {
            StreamResourceInfo resource = getUriDataByPath(_path);

            return resource != null;
        }

        public string getFileContent()
        {
            string result = "";
            if (checkIfFileExists())
            {
                StreamResourceInfo resource = getUriDataByPath(_path);
                using (StreamReader reader = new StreamReader(resource.Stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }

        private StreamResourceInfo getUriDataByPath(string path)
        {
            Uri uri = new Uri(path, UriKind.Absolute);

            StreamResourceInfo resource = Application.GetResourceStream(uri);
            return resource;
        }
    }
}
