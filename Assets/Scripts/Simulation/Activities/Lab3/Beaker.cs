using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab3
{
    [System.Serializable]
    public class Beaker : SimulationMixableBehavior
    {
        public int Volume { get; set; } = 0;

        public Beaker(int volume)
        {
            itemName = volume + "mL Beaker";
            if (volume == 150)
            {
                icon = Resources.Load<Sprite>("Simulation/Lab3/Equipments/150Beaker");
            }
            else if (volume == 400)
            {
                icon = Resources.Load<Sprite>("Simulation/Lab3/Equipments/400Beaker");
            }
            else
            {
                Debug.LogError("Unsupported beaker volume: " + volume);
            }

            Volume = volume;
            AutoMix = true;
        }

        public Beaker(Beaker otherItem) : base(otherItem)
        {
            Volume = otherItem.Volume;
        }
    }
}
