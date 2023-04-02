using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heart : MonoBehaviour
{
    public static Heart current;

    private bool normalPumping = true;
    private float currentTime;

    private static float heartRate;
    private static float acceleration;
    private static float radius;
    private static float dashVelocity;

    void Awake()
    {
        current = this;

        heartRate = 80.0f;
        acceleration = -20.0f;
        radius = 7.0f;
        dashVelocity = -100.0f;
    }

    public static float HeartRate
    {
        get {return heartRate; }
        set {heartRate = value; }
    }

    public static float AccelRate
    {
        get {return acceleration; }
        set {acceleration = value; }
    }

    public static float Radius
    {
        get {return radius; }
        set {radius = value; }
    }

    public static float DashVelocity
    {
        get {return dashVelocity; }
        set {dashVelocity = value; }
    }

    public event Action<float> onPump;
    public void Pump(float AccelRate)
    {
        if (onPump != null)
        {
            onPump(AccelRate);
        }
    }

    void FixedUpdate()
    {
        if (normalPumping)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= 60.0f / HeartRate)
            {
                currentTime = 0.0f;
                Pump(AccelRate);
            }
        }
    }
}
