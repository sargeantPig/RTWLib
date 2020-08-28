using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTWLib.Objects.Descr_strat
{

    public interface ICharacterRecord
    {
        string name { get; set; }
        string gender { get; set; }
        int command { get; set; }
        int influence { get; set; }
        int management { get; set; }
        int subterfuge { get; set; }
        int age { get; set; }
        string status { get; set; }
        string leader { get; set; }

    }

    public class CharacterRecord : ICharacterRecord
    {
        public string name { get; set; }
        public string gender { get; set; }
        public int command { get; set; }
        public int influence { get; set; }
        public int management { get; set; }
        public int subterfuge { get; set; }
        public int age { get; set; }
        public string status { get; set; }
        public string leader { get; set; }

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

        virtual public string Output()
        {
            return "character_record\t\t" + name + ", \t" + gender + ", command " + command.ToString() + ", influence " + influence.ToString() + ", management " + management.ToString() +
                ", subterfuge " + subterfuge.ToString() + ", age " + age.ToString() + ", " + status + ", " + leader + " " + "\r\n";

        }
    }
}
