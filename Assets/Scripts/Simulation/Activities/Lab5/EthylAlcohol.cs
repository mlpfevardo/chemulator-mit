﻿using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab5
{
    [System.Serializable]
    public class EthylAlcohol : SimulationMixableBehavior
    {
        public EthylAlcohol()
        {
            itemName = "Ethyl Alcohol";
            icon = GameStateManagerScript.LoadAsset<Sprite>("Simulation/Lab5/Materials/Ethyl");
        }

        public EthylAlcohol(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
