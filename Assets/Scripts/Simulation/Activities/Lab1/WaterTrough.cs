using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab1
{
    [System.Serializable]
    public class WaterTrough : SimulationMixableBehavior
    {
        private float currentTime = 0f;
        private int count = 0;

        public WaterTrough()
        {
            itemName = "Water Trough";
            icon = Resources.Load<Sprite>("Simulation/Lab1/Materials/WaterTrough");
            Scale = 70f;
            AutoMix = true;
        }

        public WaterTrough(WaterTrough otherItem) : base(otherItem)
        {
            currentTime = otherItem.currentTime;
            count = otherItem.count;
        }

        public override bool DoMix(List<SimulationMixableBehavior> otherMixables, DropZoneObjectHandler dropZoneObject, DraggableObjectBehavior draggedObject = null, List<SimulationMixableBehavior> draggedMixables = null)
        {
            if (draggedObject != null)
            {
                if (draggedObject.GetType() == typeof(Beaker))
                {
                    if ((draggedMixables.Find(m => m.GetType() == typeof(Naphthalene)) != null && draggedMixables.Find(m => m.GetType() == typeof(BlueDye)) != null) ||
                        (draggedMixables.Find(m => m.GetType() == typeof(Ice)) != null) || 
                        (draggedObject.MixtureItem as Beaker).Volume == 400)
                    {
                        count++;

                        if (count >= 3)
                        {

                            ImageAnimationManager.CreateLoopingAnimation(79, Parent.transform, () =>
                            {
                                if (GameTimerScript.Instance.GetMinutes() - currentTime >= 0) // 5 minute
                            {

                                    return true;
                                }

                                return false;
                            }, true);

                            return true;
                        }
                    }
                    else
                    {
                        ModalPanel.Instance.ShowModalOK("No duplicate", "This item has already been added");
                    }
                }
            }

            return false;
        }
    }
}
