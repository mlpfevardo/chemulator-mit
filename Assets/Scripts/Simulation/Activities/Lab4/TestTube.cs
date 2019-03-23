using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab4
{
    [System.Serializable]
    public class TestTube : SimulationMixableBehavior
    {
        public TestTube()
        {
            this.itemName = "Test Tube";
            this.icon = Resources.Load<Sprite>("Simulation/Lab4/Equipments/Tube");
        }

        public TestTube(SimulationMixableBehavior other) : base(other)
        {

        }
    }
}
