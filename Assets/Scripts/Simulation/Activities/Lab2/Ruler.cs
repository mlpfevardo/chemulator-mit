using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab2
{
    [System.Serializable]
    public class Ruler : SimulationMixableBehavior
    {
        public Ruler()
        {
            itemName = "Ruler";
            icon = GameStateManagerScript.LoadAsset<Sprite>("Simulation/Lab2/Equipments/Ruler");
        }

        public Ruler(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
