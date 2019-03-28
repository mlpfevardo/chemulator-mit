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
        public bool IsAvailable { get; set; } = false;

        private bool isMixing = false;
        private float currentTime;
        private bool hasCACL = false;
        private bool hasNACO = false;

        public Beaker(int volume)
        {
            itemName = volume + "mL Beaker";
            if (volume == 150)
            {
                icon = GameStateManagerScript.LoadAsset<Sprite>("Simulation/Lab3/Equipments/150Beaker");
            }
            else if (volume == 400)
            {
                icon = GameStateManagerScript.LoadAsset<Sprite>("Simulation/Lab3/Equipments/400Beaker");
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

        public override bool DoMix(List<SimulationMixableBehavior> otherMixables, DropZoneObjectHandler dropZoneObject, DraggableObjectBehavior draggedObject = null, List<SimulationMixableBehavior> draggedMixables = null)
        {
            if (draggedObject != null)
            {
                if (Volume == 150)
                {
                    if (!isMixing)
                    {
                        if (draggedObject.MixtureItem.GetType() == typeof(Cylinder))
                        {
                            if (draggedMixables.Find(m => m.GetType() == typeof(CalciumChloride)) != null)
                            {
                                ImageAnimationManager.CreateAnimation(93, Parent.transform);
                                hasCACL = true;

                                if (hasCACL && hasNACO)
                                {
                                    AutoMix = false;
                                    MixButtonTitle = "Stir";
                                }

                                draggedObject.SetRemoveOnEnd();

                                return true;
                            }
                            else if (draggedMixables.Find(m => m.GetType() == typeof(SodiumCarbonate)) != null)
                            {
                                ImageAnimationManager.CreateAnimation(93, Parent.transform);
                                hasNACO = true;

                                if (hasCACL && hasNACO)
                                {
                                    AutoMix = false;
                                    MixButtonTitle = "Stir";
                                }

                                draggedObject.SetRemoveOnEnd();

                                return true;
                            }
                            else
                            {
                                ModalPanel.Instance.ShowModalOK("Invalid Item", "You can not add this item to the beaker");
                            }
                        }
                    }
                }
            }
            else
            {
                isMixing = true;
                currentTime = GameTimerScript.Instance.GetMinutes();

                ImageAnimationManager.CreateLoopingAnimation(96, Parent.transform, () =>
                {
                    if (GameTimerScript.Instance.GetMinutes() - currentTime >= 5) // 5 minutes
                    {
                        isMixing = false;
                        AutoMix = false;
                        IsAvailable = true;
                        return true;
                    }

                    return false;
                }, true);
            }

            return false;
        }
    }
}
