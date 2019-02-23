using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab3
{
    public class CalciumChloride : SimulationMixableBehavior
    {
        public CalciumChloride()
        {
            itemName = "1M Calcium Chloride";
            icon = Resources.Load<Sprite>("Simulation/Lab3/Materials/CalciumChloride");
        }

        public CalciumChloride(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
