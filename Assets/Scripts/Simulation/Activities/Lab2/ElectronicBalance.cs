using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Simulation.Activities.Lab2
{
    [System.Serializable]
    public class ElectronicBalance : SimulationMixableBehavior
    {
        public ElectronicBalance()
        {
            itemName = "Electronic Balance";
            icon = Resources.Load<Sprite>("Simulation/Lab2/Equipments/ElectronicBalance");
            AutoMix = true;
            Scale = 40;
        }

        public ElectronicBalance(SimulationMixableBehavior otherItem) : base(otherItem)
        {
        }

        public override bool DoMix(List<SimulationMixableBehavior> otherMixables, DropZoneObjectHandler dropZoneObject, DraggableObjectBehavior draggedObject = null, List<SimulationMixableBehavior> draggedMixables = null)
        {
            if (draggedObject != null)
            {
                if (draggedObject.MixtureItem.GetType() == typeof(Coins))
                {
                    ModalPanel.Instance.ShowModalOK("Electronic Balance", "The coins weigh 60 grams");
                }
                else if (draggedObject.MixtureItem.GetType() == typeof(Cylinder))
                {
                    Water water = SimulationMixtureManager.instance.GetSavedMixtures(draggedObject.MixtureItem).FirstOrDefault(m => m.GetType() == typeof(Water)) as Water;

                    if (water != null)
                    {
                        ModalPanel.Instance.ShowModalOK("Electronic Balance", "This weighs " + (water.Volume + 35) +  " grams");
                    }
                    else
                    {
                        ModalPanel.Instance.ShowModalOK("Electronic Balance", "This weighs 34.9 grams");
                    }
                }
            }

            return false;
        }
    }
}
