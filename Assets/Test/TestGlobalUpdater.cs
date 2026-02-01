using CCLBStudio.GlobalUpdater;
using UnityEngine;

public class TestGlobalUpdater : UpdatableMonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public override void Tick()
    {
        Debug.Log("tick !");
    }

    public override void FixedTick()
    {
        Debug.Log("fixed tick !");
    }

    public override void LateTick()
    {
        Debug.Log("late tick !");
    }
}
