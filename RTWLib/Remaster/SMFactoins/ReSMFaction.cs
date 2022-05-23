using RTWLib.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Functions.Remaster
{
    public class ReSMFaction
    {
        readonly static string divisor = ";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;";

        public ReSMFBase<string> faction, name, descr, culture, ethnicity, men, women, surnames,
            loadScrIcon, stratSymModel, sratRebSymModel,
            intro, victory, defeat, aiPersona;
           
        public ReSMFBase<object[]> tags;

        public ReSMFBase<int> stdIndex, smallLogoInd, triumphVal, rebLogoInd, logoInd, rebStdInd;

        public ReSMFBase<bool> custBattleAvailable, canSap, navalInvader, reproduction;

        public MapColour factionColour, familyTree;

        public ReSMFaction()
        { 
        
        }

        public string Output()
        {
            return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}\r\n\r\n" ,
                faction.Output(),
                culture.Output(),

                factionColour.Output(),
                stdIndex.Output(),
                logoInd.Output(),
                smallLogoInd.Output(),
                triumphVal.Output(),
                custBattleAvailable.Output(),
                canSap.Output(),
                navalInvader.Output(),
                divisor);
        }

    }
}
