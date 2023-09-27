using LabLuby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAnimation : MonoBehaviour
{
    [SerializeField] private UnitController controller;

    public void OnFinishLedgeClimb()
    {
        controller.FinishLedgeClimb();
    }
}
