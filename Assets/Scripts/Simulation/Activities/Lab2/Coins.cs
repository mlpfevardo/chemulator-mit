using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab2
{
    public class Coins : SimulationMixableBehavior
    {
        public Coins()
        {
            itemName = "10 One-Peso Coins";
            icon = Resources.Load<Sprite>("Simulation/Lab2/Materials/Coins");
            AutoMix = true;
        }

        public Coins(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }

        public override bool DoMix(List<SimulationMixableBehavior> otherMixables, DropZoneObjectHandler dropZoneObject, DraggableObjectBehavior draggedObject = null)
        {
            if (draggedObject != null)
            {
                if (draggedObject.MixtureItem.GetType() == typeof(Ruler))
                {
                    ModalPanel.Instance.ShowModalOK("Ruler", "The height of the coin stack is 2.05 cm. Diaemeter is 2.30cm");
                }
            }

            return false;
        }

        public override void OnDrop()
        {
            ImageAnimationManager.Instance.ShowAnimation(82);
        }
    }
}
