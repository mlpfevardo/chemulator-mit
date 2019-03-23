using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab1
{
    [System.Serializable]
    public class Filter : SimulationMixableBehavior
    {
        public Filter()
        {
            itemName = "Filter";
            icon = Resources.Load<Sprite>("Simulation/Lab1/Equipments/Filter");
            Scale = 20f;
        }

        public Filter(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
