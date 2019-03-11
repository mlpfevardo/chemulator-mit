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

        public override bool DoMix(List<SimulationMixableBehavior> otherMixables, DropZoneObjectHandler dropZoneObject, DraggableObjectBehavior draggedObject = null)
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
                            ImageAnimationManager.Instance.ShowAnimation(16);

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
                                    ImageAnimationManager.Instance.ShowAnimation(12);

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
                        if (otherMixables.Find(m => m.GetType() == typeof(Water)) != null)
                        {
                            if (otherMixables.Find(m => m.GetType() == typeof(Sand)) != null)
                            {
                                ModalPanel.Instance.ShowModalOK("Result", "The supernatant liquid is clear");
                            }
                            else if (otherMixables.Find(m => m.GetType() == typeof(Chalk)) != null)
                            {
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
            }

            return false;
        }
    }
}
