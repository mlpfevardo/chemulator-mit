using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab1
{
    public class DistillationSetup : SimulationMixableBehavior
    {
        public DistillationSetup()
        {
            itemName = "Distillation Setup";
            icon = Resources.Load<Sprite>("Simulation/Lab1/Equipments/DistillationSetup");
            Scale = 70f;
        }

        public DistillationSetup(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
