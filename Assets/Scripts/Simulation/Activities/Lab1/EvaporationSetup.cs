using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab1
{
    public class EvaporationSetup : SimulationMixableBehavior
    {
        public EvaporationSetup()
        {
            itemName = "Evaporation Setup";
            icon = Resources.Load<Sprite>("Simulation/Lab1/Equipments/EvaporationSetup");
            Scale = 70f;
            AutoMix = true;
        }

        public EvaporationSetup(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }

        public override bool DoMix(List<SimulationMixableBehavior> otherMixables, DropZoneObjectHandler dropZoneObject, DraggableObjectBehavior draggedObject = null, List<SimulationMixableBehavior> draggedMixables = null)
        {
            if (draggedObject != null)
            {
                if (LabOneManager.ActivePart == LabOneManager.LabPart.PartB)
                {
                    if (draggedObject.MixtureItem.GetType() == typeof(FiltrationSetup) &&
                        (draggedObject.MixtureItem as FiltrationSetup).FilterComplete)
                    {
                        //draggedObject.SetRemoveOnEnd();

                        ImageAnimationManager.CreateAnimation(32, this.Parent.transform, () =>
                        {
                            ImageAnimationManager.CreateAnimation(33, Parent.transform, null, false);
                        });

                        return true;
                    }
                }
            }

            return false;
        }
    }
}
