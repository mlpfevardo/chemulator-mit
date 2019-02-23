using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab2
{
    public class Thermometer : SimulationMixableBehavior
    {
        public Thermometer()
        {
            itemName = "Thermometer";
            icon = Resources.Load<Sprite>("Simulation/Lab2/Equipments/Thermometer");
        }

        public Thermometer(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
