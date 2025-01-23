using DigitalRuby.RainMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainManager : MonoBehaviour
{
    public BaseRainScript RainScript;
    public float MinRainDuration = 5.0f;
    public float MaxRainDuration = 10.0f;
    public float MinDryDuration = 20.0f;
    public float MaxDryDuration = 40.0f;

    // Public read-only variable for other scripts
    public bool IsRaining { get; private set; }

    private float rainTimer;
    private float dryTimer;

    private void Start()
    {
        if (RainScript == null)
        {
            Debug.LogError("RainScript is not assigned!");
            this.enabled = false;
            return;
        }
        RainScript.EnableWind = false;
        //ScheduleNextDryPeriod();
    }

    private void Update()
    {
        if (IsRaining)
        {
            //UpdateRain();
        }
        else
        {
            //UpdateDry();
        }
    }

    private void UpdateRain()
    {
        if (rainTimer > 0)
        {
            rainTimer -= Time.deltaTime;
        }
        else
        {
            StopRain();
            ScheduleNextDryPeriod();
        }
    }

    private void UpdateDry()
    {
        if (dryTimer > 0)
        {
            dryTimer -= Time.deltaTime;
        }
        else
        {
            StartRain();
            ScheduleNextRainPeriod();
        }
    }

    public void StartRain()
    {
        RainScript.RainIntensity = 0.5f;
        RainScript.EnableWind = true;
        IsRaining = true;
    }

    public void StopRain()
    {
        RainScript.RainIntensity = 0.0f;
        RainScript.EnableWind = false;
        IsRaining = false;
    }

    private void ScheduleNextRainPeriod()
    {
        rainTimer = Random.Range(MinRainDuration, MaxRainDuration);
    }

    private void ScheduleNextDryPeriod()
    {
        dryTimer = Random.Range(MinDryDuration, MaxDryDuration);
    }
}
