using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab1
{
    [System.Serializable]
    public class BoilSetup : SimulationMixableBehavior
    {
        public BoilSetup()
        {
            itemName = "Boiling Setup";
            icon = GameStateManagerScript.LoadAsset<Sprite>("Simulation/Lab1/Equipments/BoilSetup");
            Scale = 70f;
        }

        public BoilSetup(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
