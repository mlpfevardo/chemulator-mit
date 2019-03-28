using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab3
{
    [System.Serializable]
    public class ElectronicBalance : SimulationMixableBehavior
    {
        public ElectronicBalance()
        {
            itemName = "Electronic Balance";
            icon = GameStateManagerScript.LoadAsset<Sprite>("Simulation/Lab3/Equipments/ElectronicBalance");
            AutoMix = true;
            Scale = 40;
        }

        public ElectronicBalance(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
