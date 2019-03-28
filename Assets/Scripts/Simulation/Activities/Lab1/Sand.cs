using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab1
{
    [System.Serializable]
    public class Sand : SimulationMixableBehavior
    {
        public Sand()
        {
            itemName = "1g Sand";
            icon = GameStateManagerScript.LoadAsset<Sprite>("Simulation/Lab1/Materials/Sand");
        }

        public Sand(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
