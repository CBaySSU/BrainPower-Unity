using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class DotTruck : System.Object
{
    public WheelCollider leftWheel;
    public GameObject leftWheelMesh;
    public WheelCollider rightWheel;
    public GameObject rightWheelMesh;
    public bool motor;
    public bool steering;
    public bool reverseTurn;
}

public class DotTruckController : MonoBehaviour
{
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public List<DotTruck> truck_Infos;

    public Slider slider;

    private float verticalSpeed;
    
    private void FixedUpdate()
    {
        float motor = maxMotorTorque * verticalSpeed;
        slider.value = verticalSpeed;
        float steering = maxSteeringAngle * Input.acceleration.x;

        float brakeTorque = Mathf.Abs(Input.GetAxis("Jump"));
        if (brakeTorque > 0.001)
        {
            brakeTorque = maxMotorTorque;
            motor = 0;
        }
        else
        {
            brakeTorque = 0;
        }

        foreach (DotTruck truckInfo in truck_Infos)
        {
            if (truckInfo.steering)
            {
                truckInfo.leftWheel.steerAngle =
                    truckInfo.rightWheel.steerAngle = ((truckInfo.reverseTurn) ? -1 : 1) * steering;
            }

            if (truckInfo.motor)
            {
                truckInfo.leftWheel.motorTorque = motor;
                truckInfo.rightWheel.motorTorque = motor;
            }

            truckInfo.leftWheel.brakeTorque = brakeTorque;
            truckInfo.rightWheel.brakeTorque = brakeTorque;

            VisualizeWheel(truckInfo);
        }
    }
    
    public void VisualizeWheel(DotTruck wheelPair)
    {
        Quaternion rot;
        Vector3 pos;
        wheelPair.leftWheel.GetWorldPose(out pos, out rot);
        wheelPair.leftWheelMesh.transform.position = pos;
        wheelPair.leftWheelMesh.transform.rotation = rot;
        wheelPair.rightWheel.GetWorldPose(out pos, out rot);
        wheelPair.rightWheelMesh.transform.position = pos;
        wheelPair.rightWheelMesh.transform.rotation = rot;
    }

    public void SpeedFromAndroid(string dataReceived)
    {
        verticalSpeed = Convert.ToSingle(dataReceived);
    }
}