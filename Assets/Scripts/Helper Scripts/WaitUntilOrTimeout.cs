using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Yield instruction that will wait until a condition, or timeout after a set period
/// </summary>
public class WaitUntilOrTimeout : CustomYieldInstruction
{
    float pauseTime;
    Func<bool> myChecker;
    Action<float> onInterrupt;

    public WaitUntilOrTimeout(Func<bool> myChecker, float pauseTime,
            Action<float> onInterrupt = null)
    {
        this.myChecker = myChecker;
        this.pauseTime = pauseTime;
        this.onInterrupt = onInterrupt;
    }

    public override bool keepWaiting
    {
        get
        {
            bool checkFinished = myChecker();

            this.pauseTime -= Time.deltaTime;

            if (onInterrupt != null && checkFinished)
            {
                onInterrupt(pauseTime);
            }

            if (this.pauseTime <= 0 || checkFinished)
            {
                return false;
            }

            return true;
        }
    }
}