using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab1
{
    public class Charcoal : SimulationMixableBehavior
    {
        public Charcoal()
        {
            itemName = "Powdered Charcoal";
            icon = Resources.Load<Sprite>("Simulation/Lab1/Materials/Charcoal");
        }

        public Charcoal(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
