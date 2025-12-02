using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(MultiGraphics))]
public class MultiGraphicsButton : Button
{
    private Selectable selectable => GetComponent<Selectable>();

    private MultiGraphics graphics => GetComponent<MultiGraphics>();


    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);

        Color targetColor = colors.normalColor;
        float duration = instant ? 0f : colors.fadeDuration;

        switch (state)
        {
            case SelectionState.Normal:
                targetColor = colors.normalColor;
                break;
            case SelectionState.Highlighted:
                targetColor = colors.highlightedColor;
                break;
            case SelectionState.Pressed:
                targetColor = colors.pressedColor;
                break;
            case SelectionState.Disabled:
                targetColor = colors.disabledColor;
                break;
            case SelectionState.Selected:
                targetColor = colors.selectedColor;
                break;
        }

        foreach (var g in graphics.dynamicGraphicsColored)
            if (g != null)
                g.CrossFadeColor(targetColor, duration, true, true);

        foreach (var g in graphics.dynamicGraphicsUncolored)
            if (g != null)
                g.CrossFadeColor(targetColor, duration, true, true);
    }
}