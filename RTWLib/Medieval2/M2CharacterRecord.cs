using RTWLib.Objects.Descr_strat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Medieval2
{
    class M2CharacterRecord : CharacterRecord, ICharacterRecord
    {

        public M2CharacterRecord(CharacterRecord cr) : base(cr)
        { }

        public M2CharacterRecord()
        { }

        public override string Output()
        {
            return "character_record\t\t" + name + ", \t" + gender + ", age " + age.ToString() + ", " + status + ", " + leader + " " + "\r\n";
        }

    }
}
