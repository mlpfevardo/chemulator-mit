using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab3
{
    [System.Serializable]
    public class Cylinder : SimulationMixableBehavior
    {
        public Cylinder()
        {
            itemName = "50mL Graduated Cylinder";
            icon = GameStateManagerScript.LoadAsset<Sprite>("Simulation/Lab3/Equipments/Cylinder");
            AutoMix = true;
        }

        public Cylinder(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
