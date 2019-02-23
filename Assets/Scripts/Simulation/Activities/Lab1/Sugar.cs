using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab1
{
    public class Sugar : SimulationMixableBehavior
    {
        public Sugar()
        {
            itemName = "Brown Sugar";
            icon = Resources.Load<Sprite>("Simulation/Lab1/Materials/Sugar");
        }

        public Sugar(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
