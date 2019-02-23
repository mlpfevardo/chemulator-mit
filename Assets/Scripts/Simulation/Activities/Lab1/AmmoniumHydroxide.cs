using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab1
{
    public class AmmoniumHydroxide : SimulationMixableBehavior
    {
        public AmmoniumHydroxide()
        {
            itemName = "Ammonium Hydroxide";
            icon = Resources.Load<Sprite>("Simulation/Lab1/Materials/AmmoniumHydroxide");
        }

        public AmmoniumHydroxide(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
