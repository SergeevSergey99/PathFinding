using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class mouseInput : MonoBehaviour, IPointerClickHandler
{
    private arrayGraph ag;
    private int x;
    private int y;
    public void setArrayGraph(arrayGraph AG, int x, int y)
    {
        ag = AG;
        this.x = x;
        this.y = y;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            ag.startTarget = new Vector2(x, y);
        else if (eventData.button == PointerEventData.InputButton.Right)
            ag.endTarget = new Vector2(x, y);
    }
}
