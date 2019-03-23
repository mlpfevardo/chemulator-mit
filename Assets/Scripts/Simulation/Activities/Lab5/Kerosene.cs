using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab5
{
    [System.Serializable]
    public class Kerosene : SimulationMixableBehavior
    {
        public Kerosene()
        {
            itemName = "Kerosene";
            icon = Resources.Load<Sprite>("Simulation/Lab5/Materials/Kerosene");
        }

        public Kerosene(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
