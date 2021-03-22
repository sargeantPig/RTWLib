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

namespace RTWLib.Functions
{
    public class SMFaction
    {
        readonly static string divisor = ";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;";

        public SMFBase<string> faction, culture, symbol,
            rebelSymbol, loadingLogo, introMovie, victoryMovie,
            defeatMovie, deathmovie;

        public SMFBase<int> stdIndex, smallLogoInd, triumphVal, logoInd;

        public SMFBase<bool> custBattleAvailable, canSap, navalInvader;

        public MapColour factionColour = new MapColour();

        public SMFaction()
        { 
        
        }

        public string Output()
        {
            return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}\n\n" ,
                faction.Output(),
                culture.Output(),
                symbol.Output(),
                rebelSymbol.Output(),
                factionColour.Output(),
                loadingLogo.Output(),
                stdIndex.Output(),
                logoInd.Output(),
                smallLogoInd.Output(),
                triumphVal.Output(),
                introMovie.Output(),
                victoryMovie.Output(),
                defeatMovie.Output(),
                deathmovie.Output(),
                custBattleAvailable.Output(),
                canSap.Output(),
                navalInvader.Output(),
                divisor);
        }

    }
}
