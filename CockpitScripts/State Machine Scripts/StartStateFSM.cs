using UnityEngine;

public class StartStateFSM : FlightFSM
{
    Rigidbody planeRb;

    public StartStateFSM(FlightStateManager stateManager, State state, FlightValuesSO flightValues, Rigidbody rb):base(stateManager,state, flightValues) {
        planeRb = rb;
    }

    public override void InitState()
    {
        EnteredNewState(stateManager, State.Start);
        UpdateInteractable();
    }

    public override void Update(float dt)
    {
        if (flightValues.GetPlaneVelocityInKnots() > flightValues.TakeOffSpeed)
        {
            UpdateState();
        }
    }

    public override void UpdateState()
    {
        stateManager.SwitchState(State.TakeOff);
    }

}

