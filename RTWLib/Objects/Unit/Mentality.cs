using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTWLib.Data;
namespace RTWLib.Objects
{
    public class Mentality
    {
        /// <summary>
        /// Base morale
        /// </summary>
        public int morale;
        /// <summary>
        /// discipline of the unit (normal, low, disciplined or impetuous)
        /// </summary>
        public Statmental_discipline discipline;
        /// <summary>
        /// training of the unit (how tidy the formation is)
        /// </summary>
        public Statmental_training training;

        public Mentality()
        { }
    }

}
