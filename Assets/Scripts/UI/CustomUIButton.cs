using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class CustomUIButton : CustomUI {

    TextMeshProUGUI text;
    Image image;
    Button button;

    public void SetText(string newText)
    {
        text.SetText(newText);
    }

    public void ClearListeners()
    {
        button.onClick.RemoveAllListeners();
    }

    public void AddClickListener(UnityAction action)
    {
        button.onClick.AddListener(action);
    }

    protected override void OnSkinUI()
    {
        base.OnSkinUI();

        image = GetComponent<Image>();
        button = GetComponent<Button>();

        button.transition = Selectable.Transition.SpriteSwap;
        button.targetGraphic = image;

        image.sprite = skinData.buttonSprite;
        image.type = Image.Type.Sliced;
        button.spriteState = skinData.buttonSpriteState;

        if (transform.Find("Text") == null)
        {
            var obj = new GameObject("Text");
            text = obj.AddComponent<TextMeshProUGUI>();
            obj.transform.SetParent(this.transform, false);
            text.SetText("Mix");
        }
        else
        {
            text = transform.GetComponentInChildren<TextMeshProUGUI>();
        }
    }
}
