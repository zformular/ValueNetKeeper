using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NetKeeper.Commom
{
    public class CommonVar
    {
        /// <summary>
        ///  默认跟目录
        /// </summary>
        public static String BaseUrl = AppDomain.CurrentDomain.BaseDirectory;

        public static String FileName = Path.Combine(BaseUrl, "Data\\data.xml");
    }
}
