
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab2
{
    public class Water : SimulationMixableBehavior
    {
        public int Volume { get; set; } = 0;
        public Water()
        {
            itemName = "Distilled Water";
            icon = Resources.Load<Sprite>("Simulation/Lab2/Materials/Water");
        }

        public Water(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
