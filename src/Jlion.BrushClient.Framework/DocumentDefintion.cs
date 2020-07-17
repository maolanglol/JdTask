using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Jlion.BrushClient.Framework
{
    public class DocumentDefintion
    {
        /// <summary>
        /// 获得文档位置
        /// </summary>
        public static string DocumentPath
        {
            get
            {
                var myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (string.IsNullOrEmpty(myDocuments))
                    myDocuments = @"C:\Document";
                return Path.Combine(myDocuments, "Brush Plugins");
            }
        }
    }
}
