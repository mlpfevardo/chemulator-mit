using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab5
{
    [System.Serializable]
    public class SodiumChloride : SimulationMixableBehavior
    {
        public SodiumChloride()
        {
            itemName = "0.15g Sodium Chloride";
            icon = GameStateManagerScript.LoadAsset<Sprite>("Simulation/Lab5/Materials/SoChlo");
        }

        public SodiumChloride(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }
    }
}
