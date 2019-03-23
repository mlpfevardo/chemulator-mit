using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab5
{
    [System.Serializable]
    public class MicroTestTube : SimulationMixableBehavior
    {
        public MicroTestTube()
        {
            this.itemName = "Test Tube";
            this.icon = Resources.Load<Sprite>("Simulation/Lab5/Equipments/Tube");
        }

        public MicroTestTube(MicroTestTube other) : base(other)
        {

        }
    }
}