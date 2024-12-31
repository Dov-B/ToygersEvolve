using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAndClick : MonoBehaviour
{
   [SerializeField] private List<GameObject> _choicesList = new List<GameObject>();

    public void TestButton()
    {
        Debug.Log("button pressed");
    }


}
