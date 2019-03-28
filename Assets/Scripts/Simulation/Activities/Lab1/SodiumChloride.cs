using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab1
{
    [System.Serializable]
    public class SodiumChloride : SimulationMixableBehavior
    {
        public SodiumChloride()
        {
            itemName = "Sodium Chloride";
            icon = GameStateManagerScript.LoadAsset<Sprite>("Simulation/Lab1/Materials/SodiumChloride");
        }

        public SodiumChloride(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
