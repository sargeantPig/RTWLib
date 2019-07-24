using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Data;
namespace RTWLib.Objects
{
    public class Formation
    {
        /// <summary>
        /// [0] soldier spacing (in meters) side to side
        /// [1] soldier spacing back to back
        /// </summary>
        public float[] FormationTight;
        /// <summary>
        /// [0] soldier spacing (in meters) side to side
        /// [1] soldier spacing back to back
        /// </summary>
        public float[] FormationSparse;
        /// <summary>
        /// number of ranks in the formation
        /// </summary>
        public int FormationRanks;
        /// <summary>
        /// special formations that are possible. one or two of square, horde, phalanx, testudo or wedge
        /// </summary>
        public FormationTypes FormationFlags;

        public Formation()
        {
            FormationTight = new float[2];
            FormationSparse = new float[2];
        }
    }
}
