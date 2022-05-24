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

        public ReSMFBase<int> stdIndex, rebLogoInd, logoInd, rebStdInd;

        public ReSMFBase<bool> custBattleAvailable, navalInvader, reproduction;

        public MapColour factionColour, familyTree;

        public List<object> objects = new List<object>();



        public ReSMFaction()
        { 
        
        }

        public string Output()
        {
            return string.Format("{0}\r\n\t{\r\n{1}{2}{3}{4}{5}{{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}\r\n\r\n" ,
                faction.Output(),
                name.Output(),
                descr.Output(),
                culture.Output(),
                ethnicity.Output(),

                factionColour.Output(),
                stdIndex.Output(),
                logoInd.Output(),

                custBattleAvailable.Output(),

                navalInvader.Output(),
                divisor);
        }

    }
}
