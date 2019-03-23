using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab1
{
    [System.Serializable]
    public class BlueDye : SimulationMixableBehavior
    {
        public BlueDye()
        {
            itemName = "Blue Dye";
            icon = Resources.Load<Sprite>("Simulation/Lab1/Materials/BlueDye");
        }

        public BlueDye(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
