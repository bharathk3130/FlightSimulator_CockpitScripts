using UnityEngine;

public class TakeOffStateFSM : FlightFSM
{
    Rigidbody planeRb;

    public TakeOffStateFSM(FlightStateManager stateManager, State state, FlightValuesSO flightValues, Rigidbody planeRb) : base(stateManager, state, flightValues)
    {
        this.planeRb = planeRb;
    }

    //public override void InitState()
    //{

    ////    Debug.Log("INSIDE TAKEOFF STATE");
    ////    currentSubState = TakeOffSubState.Lights;
    ////    stateManager.takeoffSubStateMap[currentSubState].Initialize();
    ////    Debug.Log("turn on lights");

    //}

    public override void Update(float dt)
    {
        if (flightValues.GetAltitude() > flightValues.ClimbAltitude)
        {
            UpdateState();
        }
    }

    public override void InitState()
    {
        EnteredNewState(stateManager, State.TakeOff);
        UpdateInteractable();
    }

    public override void UpdateState()
    {
        stateManager.SwitchState(State.Climb);
    }
}
