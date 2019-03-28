using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab3
{
    [System.Serializable]
    public class WatchGlass : SimulationMixableBehavior
    {
        public WatchGlass()
        {
            itemName = "Spatula";
            icon = GameStateManagerScript.LoadAsset<Sprite>("Simulation/Lab3/Equipments/WatchGlass");
        }

        public WatchGlass(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
