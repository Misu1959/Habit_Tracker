using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PageColorizer : MonoBehaviour
{

    private MultiGraphics[] objectToColorize;

    private void Awake()
    {
        objectToColorize = GetComponentsInChildren<MultiGraphics>();
    }


    public void Colorize(Color32 newColor)
    {
        foreach(MultiGraphics obj in objectToColorize)
            obj.ChangeColor(newColor);
    }

}
