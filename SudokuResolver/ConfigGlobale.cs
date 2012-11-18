using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigGlobaleNameSpace
{
    class ConfigGlobale
    {
        public string GetRepApplication()
        {
            return System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
        }
    }
}
