using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab2
{
    [System.Serializable]
    public class Water : SimulationMixableBehavior
    {
        public int Volume { get; set; } = 0;
        public Water()
        {
            itemName = "Distilled Water";
            icon = GameStateManagerScript.LoadAsset<Sprite>("Simulation/Lab2/Materials/Water");
        }

        public Water(Water otherItem) : base(otherItem)
        {
            Volume = otherItem.Volume;
        }
    }
}
