using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab1
{
    public class Beaker : SimulationMixableBehavior
    {
        public int Volume { get; set; } = 0;
        public bool isAvailable = false;

        private float currentTime;
        private bool isMixing = false;

        public Beaker(int volume)
        {
            itemName = volume + "mL Beaker";
            if (volume == 150)
            {
                icon = Resources.Load<Sprite>("Simulation/Lab1/Equipments/150Beaker");
            }
            else if (volume == 400)
            {
                icon = Resources.Load<Sprite>("Simulation/Lab1/Equipments/400Beaker");
            }
            else
            {
                Debug.LogError("Unsupported beaker volume: " + volume);
            }

            Volume = volume;
            AutoMix = true;
        }

        public Beaker(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }

        public override bool DoMix(List<SimulationMixableBehavior> otherMixables, DropZoneObjectHandler dropZoneObject, DraggableObjectBehavior draggedObject = null, List<SimulationMixableBehavior> draggedMixables = null)
        {
            if (draggedObject != null)
            {
                if (LabOneManager.ActivePart == LabOneManager.LabPart.PartA)
                {
                    if (draggedObject.MixtureItem.GetType() == typeof(Water))
                    {
                        if (otherMixables.Find(m => m.GetType() == draggedObject.MixtureItem.GetType()) == null)
                        {
                            // first time water
                            ImageAnimationManager.CreateAnimation(16, dropZoneObject.transform);

                            draggedObject.SetRemoveOnEnd();
                            return true;
                        }
                        else
                        {
                            ModalPanel.Instance.ShowModalOK("Duplicate", "Only one of this item can be added");
                        }
                    }
                    else if (draggedObject.MixtureItem.GetType() == typeof(Sand))
                    {
                        if ((otherMixables.Find(m => m.GetType() == typeof(Water)) != null))
                        {
                            if (otherMixables.Find(m => m.GetType() == typeof(Chalk)) == null)
                            {
                                if (otherMixables.Find(m => m.GetType() == draggedObject.MixtureItem.GetType()) == null)
                                {
                                    // first time water
                                    ImageAnimationManager.CreateAnimation(10, dropZoneObject.transform);

                                    draggedObject.SetRemoveOnEnd();
                                    return true;
                                }
                                else
                                {
                                    ModalPanel.Instance.ShowModalOK("Duplicate", "Only one of this item can be added");
                                }
                            }
                            else
                            {
                                ModalPanel.Instance.ShowModalOK("Limit", "Only one compound can be mixed with water");
                            }
                        }
                        else
                        {
                            ModalPanel.Instance.ShowModalOK("No Water", "Water not yet added. Please add water first on the beaker");
                        }
                    }
                    else if (draggedObject.MixtureItem.GetType() == typeof(Chalk))
                    {
                        if ((otherMixables.Find(m => m.GetType() == typeof(Water)) != null))
                        {
                            if (otherMixables.Find(m => m.GetType() == typeof(Sand)) == null)
                            {
                                if (otherMixables.Find(m => m.GetType() == draggedObject.MixtureItem.GetType()) == null)
                                {
                                    // first time water
                                    ImageAnimationManager.CreateAnimation(14, dropZoneObject.transform);
                                    draggedObject.SetRemoveOnEnd();
                                    return true;
                                }
                                else
                                {
                                    ModalPanel.Instance.ShowModalOK("Duplicate", "Only one of this item can be added");
                                }
                            }
                            else
                            {
                                ModalPanel.Instance.ShowModalOK("Limit", "Only one compound can be mixed with water");
                            }
                        }
                        else
                        {
                            ModalPanel.Instance.ShowModalOK("No Water", "Water not yet added. Please add water first on the beaker");
                        }
                    }
                    else if (draggedObject.MixtureItem.GetType() == typeof(Beaker))
                    {
                        if (draggedMixables.Find(m => m.GetType() == typeof(Water)) != null)
                        {
                            if (draggedMixables.Find(m => m.GetType() == typeof(Sand)) != null)
                            {
                                ImageAnimationManager.CreateAnimation(12, dropZoneObject.transform, () => ModalPanel.Instance.ShowModalOK("Result", "The supernatant liquid is clear"));
                            }
                            else if (draggedMixables.Find(m => m.GetType() == typeof(Chalk)) != null)
                            {
                                ImageAnimationManager.CreateAnimation(16, dropZoneObject.transform, () => ModalPanel.Instance.ShowModalOK("Result", "The supernatant liquid is clear"));
                                ModalPanel.Instance.ShowModalOK("Result", "The supernatant liquid is opaque");
                            }
                            else
                            {
                                ModalPanel.Instance.ShowModalOK("Result", "This is just water");
                            }
                        }
                        else
                        {
                            ModalPanel.Instance.ShowModalOK("No liquid", "No liquid to transfer between beakers");
                        }
                    }
                }
                else if (LabOneManager.ActivePart == LabOneManager.LabPart.PartB)
                {
                    if (draggedObject.MixtureItem.GetType() == typeof(Water) && otherMixables.Find(m => m.GetType() == typeof(Water)) == null)
                    {
                        if (otherMixables.Find(m => m.GetType() == typeof(SodiumChloride)) != null &&
                            otherMixables.Find(m => m.GetType() == typeof(Chalk)) != null)
                        {
                            AutoMix = false;
                            MixButtonTitle = "Stir";
                        }
                    }
                    else if (draggedObject.MixtureItem.GetType() == typeof(SodiumChloride) && otherMixables.Find(m => m.GetType() == typeof(SodiumChloride)) == null)
                    {
                        ImageAnimationManager.CreateAnimation(21, dropZoneObject.transform);
                        if (otherMixables.Find(m => m.GetType() == typeof(Chalk)) != null &&
                            otherMixables.Find(m => m.GetType() == typeof(Water)) != null)
                        {
                            AutoMix = false;
                            MixButtonTitle = "Stir";
                        }
                    }
                    else if (draggedObject.MixtureItem.GetType() == typeof(Chalk) && otherMixables.Find(m => m.GetType() == typeof(Chalk)) == null)
                    {
                        if (otherMixables.Find(m => m.GetType() == typeof(SodiumChloride)) != null)
                        {
                            ImageAnimationManager.CreateAnimation(22, dropZoneObject.transform);
                            if (otherMixables.Find(m => m.GetType() == typeof(SodiumChloride)) != null &&
                                otherMixables.Find(m => m.GetType() == typeof(Water)) != null)
                            {
                                AutoMix = false;
                                MixButtonTitle = "Stir";
                            }
                        }
                        else
                        {
                            ModalPanel.Instance.ShowModalOK("Missing Material", "Add a sodium chloride first");
                            return false;
                        }
                    }
                    else
                    {
                        ModalPanel.Instance.ShowModalOK("Duplicate", "This material has been added to this beaker");
                        return false;
                    }

                    return true;
                }
                else if (LabOneManager.ActivePart == LabOneManager.LabPart.PartC)
                {
                    if (draggedObject.MixtureItem.GetType() == typeof(Water))
                    {
                        if (otherMixables.Find(m => m.GetType() == draggedObject.MixtureItem.GetType()) == null)
                        {
                            // first time water
                            ImageAnimationManager.CreateAnimation(37, dropZoneObject.transform);

                            draggedObject.SetRemoveOnEnd();
                            return true;
                        }
                        else
                        {
                            ModalPanel.Instance.ShowModalOK("Duplicate", "Only one of this item can be added");
                        }
                    }
                    else if (draggedObject.MixtureItem.GetType() == typeof(BlueDye))
                    {
                        if (otherMixables.Find(m => m.GetType() == draggedObject.MixtureItem.GetType()) == null)
                        {
                            if (otherMixables.Find(m => m.GetType() == typeof(Water)) == null)
                            {
                                ModalPanel.Instance.ShowModalOK("No Water", "Add water first");
                            }
                            else
                            {
                                ImageAnimationManager.CreateAnimation(39, dropZoneObject.transform);

                                draggedObject.SetRemoveOnEnd();
                                return true;
                            }
                        }
                        else
                        {
                            ModalPanel.Instance.ShowModalOK("Duplicate", "Only one of this item can be added");
                        }
                    }
                    else if (draggedObject.MixtureItem.GetType() == typeof(AmmoniumHydroxide))
                    {
                        if (otherMixables.Find(m => m.GetType() == draggedObject.MixtureItem.GetType()) == null)
                        {
                            if (otherMixables.Find(m => m.GetType() == typeof(Water)) == null)
                            {
                                ModalPanel.Instance.ShowModalOK("No Water", "Add water first");
                            }
                            else
                            {
                                ImageAnimationManager.CreateAnimation(49, dropZoneObject.transform);

                                draggedObject.SetRemoveOnEnd();
                                return true;
                            }
                        }
                        else
                        {
                            ModalPanel.Instance.ShowModalOK("Duplicate", "Only one of this item can be added");
                        }
                    }
                }
                else if (LabOneManager.ActivePart == LabOneManager.LabPart.PartD)
                {
                    if (draggedObject.MixtureItem.GetType() == typeof(Water))
                    {
                        if (otherMixables.Find(m => m.GetType() == draggedObject.MixtureItem.GetType()) == null)
                        {
                            // first time water
                            ImageAnimationManager.CreateAnimation(61, dropZoneObject.transform);

                            draggedObject.SetRemoveOnEnd();
                            return true;
                        }
                        else
                        {
                            ModalPanel.Instance.ShowModalOK("Duplicate", "Only one of this item can be added");
                        }
                    }
                    else if (draggedObject.MixtureItem.GetType() == typeof(Sugar))
                    {
                        if (otherMixables.Find(m => m.GetType() == draggedObject.MixtureItem.GetType()) == null)
                        {
                            // first time water
                            if (otherMixables.Find(m => m.GetType() == typeof(Water)) == null)
                            {
                                ModalPanel.Instance.ShowModalOK("No Water", "Add water first");
                            }
                            else
                            {
                                ImageAnimationManager.CreateAnimation(62, dropZoneObject.transform);

                                draggedObject.SetRemoveOnEnd();
                                return true;
                            }
                        }
                        else
                        {
                            ModalPanel.Instance.ShowModalOK("Duplicate", "Only one of this item can be added");
                        }
                    }
                    else if (draggedObject.MixtureItem.GetType() == typeof(Charcoal))
                    {
                        if (otherMixables.Find(m => m.GetType() == draggedObject.MixtureItem.GetType()) == null)
                        {
                            // first time water
                            if (otherMixables.Find(m => m.GetType() == typeof(Water)) == null || otherMixables.Find(m => m.GetType() == typeof(Sugar)) == null)
                            {
                                ModalPanel.Instance.ShowModalOK("Incomplete Materials", "Add water and sugar first");
                            }
                            else
                            {
                                ImageAnimationManager.CreateAnimation(65, dropZoneObject.transform);
                                AutoMix = true;
                                MixButtonTitle = "Heat & Stir";

                                draggedObject.SetRemoveOnEnd();
                                return true;
                            }
                        }
                        else
                        {
                            ModalPanel.Instance.ShowModalOK("Duplicate", "Only one of this item can be added");
                        }
                    }
                }
                else if (LabOneManager.ActivePart == LabOneManager.LabPart.PartE)
                {
                    if (draggedObject.MixtureItem.GetType() == typeof(Water) && otherMixables.Count == 0)
                    {
                        if (otherMixables.Find(m => m.GetType() == draggedObject.MixtureItem.GetType()) == null)
                        {
                            // first time water
                            ImageAnimationManager.CreateAnimation(68, dropZoneObject.transform);

                            draggedObject.SetRemoveOnEnd();

                            if (this.Volume == 400)
                            {
                                AutoMix = false;
                                MixButtonTitle = "Boil";
                            }

                            return true;
                        }
                        else
                        {
                            ModalPanel.Instance.ShowModalOK("Duplicate", "Only one of this item can be added");
                        }
                    }
                    else if (draggedObject.MixtureItem.GetType() == typeof(Ice) && otherMixables.Count == 0)
                    {
                        if (otherMixables.Find(m => m.GetType() == draggedObject.MixtureItem.GetType()) == null)
                        {
                            // first time water
                            ImageAnimationManager.CreateAnimation(78, dropZoneObject.transform);

                            draggedObject.SetRemoveOnEnd();
                            return true;
                        }
                        else
                        {
                            ModalPanel.Instance.ShowModalOK("Duplicate", "Only one of this item can be added");
                        }
                    }
                    else if (draggedObject.MixtureItem.GetType() == typeof(Naphthalene))
                    {
                        if (otherMixables.Find(m => m.GetType() == draggedObject.MixtureItem.GetType()) == null)
                        {
                            // first time water
                            ImageAnimationManager.CreateAnimation(78, dropZoneObject.transform);

                            draggedObject.SetRemoveOnEnd();
                            return true;
                        }
                        else
                        {
                            ModalPanel.Instance.ShowModalOK("Duplicate", "Only one of this item can be added");
                        }
                    }
                    else if (draggedObject.MixtureItem.GetType() == typeof(BlueDye))
                    {
                        if (otherMixables.Find(m => m.GetType() == draggedObject.MixtureItem.GetType()) == null)
                        {
                            // first time water
                            ImageAnimationManager.CreateAnimation(78, dropZoneObject.transform);

                            draggedObject.SetRemoveOnEnd();
                            return true;
                        }
                        else
                        {
                            ModalPanel.Instance.ShowModalOK("Duplicate", "Only one of this item can be added");
                        }
                    }
                    else
                    {
                        ModalPanel.Instance.ShowModalOK("Clean Beaker", "This item can only be added into an empty beaker");
                    }
                }
            }
            else
            {
                if (LabOneManager.ActivePart == LabOneManager.LabPart.PartB)
                {
                    if (!isAvailable && !isMixing)
                    {
                        currentTime = GameTimerScript.Instance.GetMinutes();
                        isMixing = true;

                        ImageAnimationManager.CreateLoopingAnimation(26, Parent.transform, () =>
                        {
                            if (GameTimerScript.Instance.GetMinutes() - currentTime >= 1) // 1 minute
                            {
                                isMixing = false;
                                isAvailable = true;
                                AutoMix = true;
                                return true;
                            }

                            return false;
                        }, true);
                    }
                }
                else if (LabOneManager.ActivePart == LabOneManager.LabPart.PartD)
                {
                    currentTime = GameTimerScript.Instance.GetMinutes();
                    isMixing = true;
                    isAvailable = true;

                    ImageAnimationManager.CreateLoopingAnimation(68, Parent.transform, () =>
                    {
                        if (GameTimerScript.Instance.GetMinutes() - currentTime >= 5) // 5 minutes
                        {
                            isMixing = false;
                            AutoMix = false;
                            return true;
                        }

                        return false;
                    }, true);
                }
                else if (LabOneManager.ActivePart == LabOneManager.LabPart.PartE)
                {
                    if (this.Volume == 400)
                    {
                        ModalPanel.Instance.ShowModalOK("Boiling", "The water is now boiling");
                    }
                }
            }

            return false;
        }
    }
}
