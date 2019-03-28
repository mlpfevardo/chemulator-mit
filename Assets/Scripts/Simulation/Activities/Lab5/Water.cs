using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab5
{
    [System.Serializable]
    public class Water : SimulationMixableBehavior
    {
        public Water()
        {
            itemName = "Distilled Water";
            icon = GameStateManagerScript.LoadAsset<Sprite>("Simulation/Lab5/Materials/Water");
        }

        public Water(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
