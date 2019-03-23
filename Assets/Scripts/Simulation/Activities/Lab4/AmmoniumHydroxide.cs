using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab4
{
    [System.Serializable]
    public class AmmoniumHydroxide : SimulationMixableBehavior
    {
        public AmmoniumHydroxide()
        {
            itemName = "2mL Ammonium Hydroxide";
            icon = Resources.Load<Sprite>("Simulation/Lab4/Materials/AmmoniumHydroxide");
        }

        public AmmoniumHydroxide(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
