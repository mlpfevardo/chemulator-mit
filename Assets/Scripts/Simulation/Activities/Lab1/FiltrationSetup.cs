using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab1
{
    [System.Serializable]
    public class FiltrationSetup : SimulationMixableBehavior
    {
        public bool FilterComplete { get; set; } = false;

        public FiltrationSetup()
        {
            itemName = "Filtration Setup";
            icon = GameStateManagerScript.LoadAsset<Sprite>("Simulation/Lab1/Equipments/FiltrationSetup");
            Scale = 70f;
            AutoMix = true;
        }

        public FiltrationSetup(FiltrationSetup otherItem) : base(otherItem)
        {
            FilterComplete = otherItem.FilterComplete;
        }

        public override bool DoMix(List<SimulationMixableBehavior> otherMixables, DropZoneObjectHandler dropZoneObject, DraggableObjectBehavior draggedObject = null, List<SimulationMixableBehavior> draggedMixables = null)
        {
            if (draggedObject != null)
            {
                if (LabOneManager.ActivePart == LabOneManager.LabPart.PartB)
                {
                    if (draggedObject.MixtureItem.GetType() == typeof(Beaker) &&
                        (draggedMixables.Find(m => m.GetType() == typeof(Water)) != null) &&
                        (draggedMixables.Find(m => m.GetType() == typeof(SodiumChloride)) != null) &&
                        (draggedMixables.Find(m => m.GetType() == typeof(Chalk)) != null))
                    { 
                        if ((draggedObject.MixtureItem as Beaker).isAvailable) 
                        {
                            ImageAnimationManager.CreateAnimation(27, this.Parent.transform, () =>
                            {
                                this.icon = GameStateManagerScript.LoadAsset<Sprite>("Simulation/Lab1/Materials/FilterComplete");
                                this.Parent.GetComponent<DropZoneObjectHandler>().SetIcon(this.icon);

                            }, false);
                            draggedObject.SetRemoveOnEnd();
                            FilterComplete = true;

                            return true;
                        }
                    }
                }
                else if (LabOneManager.ActivePart == LabOneManager.LabPart.PartD)
                {
                    if (draggedObject.MixtureItem.GetType() == typeof(Beaker))
                    {
                        if ((draggedObject.MixtureItem as Beaker).isAvailable)
                        {
                            ImageAnimationManager.CreateAnimation(70, this.Parent.transform);
                        }
                    }
                }
            }

            return false;
        }
    }
}
