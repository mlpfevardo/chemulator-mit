﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab1
{
    [System.Serializable]
    public class IronStand : SimulationMixableBehavior
    {
        public IronStand()
        {
            itemName = "Iron Stand";
            icon = GameStateManagerScript.LoadAsset<Sprite>("Simulation/Lab1/Equipments/IronStand");
        }

        public IronStand(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
