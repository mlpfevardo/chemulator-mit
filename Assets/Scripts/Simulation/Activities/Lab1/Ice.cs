using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;  

namespace Assets.Scripts.Simulation.Activities.Lab1
{
    public class Ice : SimulationMixableBehavior
    {
        public Ice()
        {
            itemName = "Ice";
            icon = Resources.Load<Sprite>("Simulation/Lab1/Materials/Ice");
        }

        public Ice(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
