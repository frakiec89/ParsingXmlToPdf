using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingXmlToPdf.Utility
{
    /// <summary>
    /// Диалоговые окна
    /// </summary>
    internal class MyDialog
    {
        internal static OpenFileDialog OfD(string filter)
        {
            var n = new OpenFileDialog();
            n.Filter = filter;
            return n;
        }

        internal static OpenFileDialog OfD()
        {
            return OfD("Все файлы (*.*)|*.*");
        }
    }
}
