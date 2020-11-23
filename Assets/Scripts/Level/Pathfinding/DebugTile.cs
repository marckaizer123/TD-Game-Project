using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugTile : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI f;
    public TextMeshProUGUI F
    {
        get { f.gameObject.SetActive(true); return f; }
        set { f = value; }
    }
    [SerializeField]
    private TextMeshProUGUI g;
    public TextMeshProUGUI G
    {
        get { g.gameObject.SetActive(true); return g; }
        set { g = value; }
    }
    [SerializeField]
    private TextMeshProUGUI h;
    public TextMeshProUGUI H
    {
        get { h.gameObject.SetActive(true); return h; }
        set { h = value; }
    }
}
