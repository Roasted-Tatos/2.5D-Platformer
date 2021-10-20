using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _points;

    public void UpdatePointsDisplay(int points)
    {
        _points.text = " " + points.ToString();
    }
}
