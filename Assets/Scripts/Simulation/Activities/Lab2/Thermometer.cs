using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab2
{
    [System.Serializable]
    public class Thermometer : SimulationMixableBehavior
    {
        public Thermometer()
        {
            itemName = "Thermometer";
            icon = GameStateManagerScript.LoadAsset<Sprite>("Simulation/Lab2/Equipments/Thermometer");
        }

        public Thermometer(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
