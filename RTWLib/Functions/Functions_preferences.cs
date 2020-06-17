using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Logger;
using RTWLib.Objects;
using System.IO;
using RTWLib.Data;
using System.Runtime.CompilerServices;

namespace RTWLib.Functions
{
    class PreferencesFile : FileBase, IFile
    {

        public PreferencesFile() 
            : base(FileNames.preferences, @"randomiser\preferences\preferences.txt", "User Preferences")
        { } 



    }
}
