using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab1
{
    [System.Serializable]
    public class DistillationSetup : SimulationMixableBehavior
    {
        public bool ForPhenolph { get; private set; } = false;

        public DistillationSetup()
        {
            itemName = "Distillation Setup";
            icon = GameStateManagerScript.LoadAsset<Sprite>("Simulation/Lab1/Equipments/DistillationSetup");
            Scale = 70f;
            AutoMix = true;
        }

        public DistillationSetup(DistillationSetup otherItem) : base(otherItem)
        {
            ForPhenolph = otherItem.ForPhenolph;
        }

        public override bool DoMix(List<SimulationMixableBehavior> otherMixables, DropZoneObjectHandler dropZoneObject, DraggableObjectBehavior draggedObject = null, List<SimulationMixableBehavior> draggedMixables = null)
        {
            if (draggedObject != null)
            {
                if (LabOneManager.ActivePart == LabOneManager.LabPart.PartC)
                {
                    if (draggedObject.MixtureItem.GetType() == typeof(Beaker) && draggedMixables.Find(m => m.GetType() == typeof(Water)) != null && (draggedMixables.Find(m => m.GetType() == typeof(BlueDye)) != null || draggedMixables.Find(m => m.GetType() == typeof(AmmoniumHydroxide)) != null))
                    {
                        draggedObject.SetRemoveOnEnd();

                        ImageAnimationManager.CreateAnimation(41, Parent.transform, () =>
                        {
                            if (draggedMixables.Find(m => m.GetType() == typeof(BlueDye)) != null)
                            {
                                ImageAnimationManager.CreateAnimation(43, Parent.transform, () =>
                                {
                                    this.icon = GameStateManagerScript.LoadAsset<Sprite>("Simulation/Lab1/Materials/Distillate1");
                                    this.Parent.GetComponent<DropZoneObjectHandler>().SetIcon(this.icon);

                                }, false);
                            }
                            else
                            {
                                ImageAnimationManager.CreateAnimation(52, Parent.transform, () =>
                                {
                                    this.icon = GameStateManagerScript.LoadAsset<Sprite>("Simulation/Lab1/Materials/Distillate2");
                                    this.Parent.GetComponent<DropZoneObjectHandler>().SetIcon(this.icon);

                                }, false);

                                ForPhenolph = true;
                            }
                        });

                        return true;
                    }
                    else if (draggedObject.MixtureItem.GetType() == typeof(Phenolphthalein))
                    {
                        if (ForPhenolph)
                        {
                            ImageAnimationManager.CreateAnimation(55, Parent.transform);
                            draggedObject.SetRemoveOnEnd();
                            return true;
                        }
                        else
                        {
                            ModalPanel.Instance.ShowModalOK("Invalid Material", "This item cannot be added.");
                        }
                    }
                }
            }
            return false;
        }
    }
}
