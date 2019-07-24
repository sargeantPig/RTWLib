using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Objects.Descr_strat
{

    public class CharacterRecord
    {
        public string name;
        public string gender;
        public int command;
        public int influence;
        public int management;
        public int subterfuge;
        public int age;
        public string status;
        public string leader;

        public CharacterRecord(CharacterRecord cr)
        {
            name = cr.name;
            gender = cr.gender;
            command = cr.command;
            influence = cr.influence;
            management = cr.management;
            subterfuge = cr.subterfuge;
            age = cr.age;
            status = cr.status;
            leader = cr.leader;
        }

        public CharacterRecord()
        { }

        public string Output()
        {
            return "character_record\t\t" + name + ", \t" + gender + ", command " + command.ToString() + ", influence " + influence.ToString() + ", management " + management.ToString() +
                ", subterfuge " + subterfuge.ToString() + ", age " + age.ToString() + ", " + status + ", " + leader + " " + "\r\n";

        }
    }
}
