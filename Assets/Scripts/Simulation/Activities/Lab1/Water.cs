using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab1
{
    [System.Serializable]
    public class Water : SimulationMixableBehavior
    {
        public Water()
        {
            itemName = "Water";
            icon = GameStateManagerScript.LoadAsset<Sprite>("Simulation/Lab1/Materials/Water");
        }

        public Water(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
