using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDebug : MonoBehaviour
{
    public static UIDebug o;

    [SerializeField]
    Text TextPush;
    [SerializeField]
    public GameObject Marker;

    void Awake()
    {
        o = this;
    }

    void Update()
    {
        TextPush.text = "";
    }

    public static void PushText(String Log)
    {
        o.TextPush.text = String.Concat(o.TextPush.text, Log, "\n");
    }

    public static void SetMarker(Vector3 Position)
    {
        o.Marker.transform.position = Position;
    }
}
