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
    public class PreferencesFile : FileBase, IFile
    {

        public PreferencesFile() 
            : base(FileNames.preferences, @"preferences\preferences.txt", "User Preferences")
        { }


        public void ImportPreferences(string dest)
        {
            File.Copy(@"preferences\preferences.txt", dest, overwrite: true);
        }


    }
}
