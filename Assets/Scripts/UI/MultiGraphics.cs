using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiGraphics : MonoBehaviour
{
    [field:SerializeField] public List<Graphic> dynamicGraphicsUncolored {  get; private set; }
    [field:SerializeField] public List<Graphic> dynamicGraphicsColored {  get; private set; }


    [field:SerializeField] public List<Graphic> staticGraphicsColored {  get; private set; }
    [field:SerializeField] public List<Graphic> staticGraphicsTransparent {  get; private set; }


    public void ChangeColor(Color32 newColor)
    {
        foreach (var graphic in dynamicGraphicsColored)
            if (graphic != null)
                graphic.color = newColor;

        foreach (var graphic in staticGraphicsColored)
            if (graphic != null)
                graphic.color = new Color32(newColor.r, newColor.g, newColor.b, 255);

        foreach (var graphic in staticGraphicsTransparent)
            if (graphic != null)
                graphic.color = new Color32(newColor.r, newColor.g, newColor.b, 64);
    }

}
