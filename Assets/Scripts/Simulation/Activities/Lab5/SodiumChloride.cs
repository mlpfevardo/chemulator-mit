﻿using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab5
{
    public class SodiumChloride : SimulationMixableBehavior
    {
        public SodiumChloride()
        {
            itemName = "0.15g Sodium Chloride";
            icon = Resources.Load<Sprite>("Simulation/Lab5/Materials/SoChlo");
        }

        public SodiumChloride(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
