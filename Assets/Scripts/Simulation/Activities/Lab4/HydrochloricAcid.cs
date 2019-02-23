using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab4
{
    public class HydrochloricAcid : SimulationMixableBehavior
    {
        public HydrochloricAcid()
        {
            itemName = "2mL Hydrochloric Acid";
            icon = Resources.Load<Sprite>("Simulation/Lab4/Materials/HydrochloricAcid");
        }

        public HydrochloricAcid(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
