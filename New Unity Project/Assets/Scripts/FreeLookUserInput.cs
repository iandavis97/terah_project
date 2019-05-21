using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class FreeLookUserInput : MonoBehaviour
{

    private bool _freeLookActive;

    // Use this for initialization
    private void Start()
    {
        CinemachineCore.GetInputAxis = GetInputAxis;
    }

    private void Update()
    {
        _freeLookActive = true;
    }

    private float GetInputAxis(string axisName)
    {
        float controller = 0;

        switch (axisName)
        {
            case "Mouse X":
                controller = Input.GetAxis("RightAnalogHorizontal");

                if (Mathf.Approximately(controller, 0) == false)
                    return controller;
                else
                    return (_freeLookActive == true) ? Input.GetAxis("Mouse X") : 0;
            case "Mouse Y":
                controller = Input.GetAxis("RightAnalogVertical");

                if (Mathf.Approximately(controller, 0) == false)
                    return controller;
                else
                    return (_freeLookActive == true) ? Input.GetAxis("Mouse Y") : 0;
        }

        return 0;
    }
}