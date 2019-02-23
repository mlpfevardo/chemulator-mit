using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab1
{
    public class Chalk : SimulationMixableBehavior
    {
        public Chalk()
        {
            itemName = "1g Chalk";
            icon = Resources.Load<Sprite>("Simulation/Lab1/Materials/Chalk");
        }

        public Chalk(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
