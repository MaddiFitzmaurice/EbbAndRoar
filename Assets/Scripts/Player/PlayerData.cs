using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public float Speed;
    public float VelocityPower;
    public float Acceleration;
    public float Decceleration;

    public PlayerData(float speed, float vPower, float accel, float deccel)
    {
        Speed = speed;
        VelocityPower = vPower;
        Acceleration = accel;
        Decceleration = deccel;
    }
}
