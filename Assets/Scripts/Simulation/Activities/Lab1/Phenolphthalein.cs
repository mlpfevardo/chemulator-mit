using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab1
{
    [System.Serializable]
    public class Phenolphthalein : SimulationMixableBehavior
    {
        public Phenolphthalein()
        {
            itemName = "Phenolphthalein";
            icon = Resources.Load<Sprite>("Simulation/Lab1/Materials/Phenolphthalein");
        }

        public Phenolphthalein(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
