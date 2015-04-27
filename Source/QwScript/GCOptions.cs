using System;

namespace QwLua
{
    [Flags]
    public enum GCOptions
    {
        Stop = 0,
        Restart = 1,
        Collect = 2,
        Count = 3,
        Countb = 4,
        Step = 5,
        SetPause = 6,
        SetStepMul = 7,
        SetMajorInc = 8,
        IsRunning = 9,
        Gen = 10,
        Inc = 11
    }
}
