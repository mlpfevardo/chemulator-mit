using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab3
{
    [System.Serializable]
    public class SodiumCarbonate : SimulationMixableBehavior
    {
        public SodiumCarbonate()
        {
            itemName = "1M Sodium Carbonate";
            icon = GameStateManagerScript.LoadAsset<Sprite>("Simulation/Lab3/Materials/SodiumCarbonate");
        }

        public SodiumCarbonate(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
