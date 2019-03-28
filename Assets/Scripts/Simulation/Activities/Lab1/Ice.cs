using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;  

namespace Assets.Scripts.Simulation.Activities.Lab1
{
    [System.Serializable]
    public class Ice : SimulationMixableBehavior
    {
        public Ice()
        {
            itemName = "Ice";
            icon = GameStateManagerScript.LoadAsset<Sprite>("Simulation/Lab1/Materials/Ice");
        }

        public Ice(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
