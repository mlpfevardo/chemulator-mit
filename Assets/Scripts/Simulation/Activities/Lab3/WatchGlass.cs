using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab3
{
    public class WatchGlass : SimulationMixableBehavior
    {
        public WatchGlass()
        {
            itemName = "Spatula";
            icon = Resources.Load<Sprite>("Simulation/Lab3/Equipments/WatchGlass");
        }

        public WatchGlass(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
