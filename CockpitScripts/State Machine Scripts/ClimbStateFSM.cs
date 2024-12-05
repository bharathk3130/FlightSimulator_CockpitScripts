using System.Numerics;
using UnityEngine;

public class ClimbStateFSM : FlightFSM
{
    //FlightStateManager stateM;
    Rigidbody planeRb;

    public ClimbStateFSM(FlightStateManager stateManager, State state, FlightValuesSO flightValues, Rigidbody planeRb) : base(stateManager, state, flightValues)
    {
        //stateM = stateManager;
        this.planeRb = planeRb;
    }

    //public override void InitState()
    //{
    //    //Debug.Log("INSIDE CLIMB STATE");
    //    //currentSubState = ClimbSubState.PullThrottle;
    //    //stateManager.climbSubStateMap[currentSubState].Initialize();
    //    //Debug.Log("pull throttle");
    //}

    public override void Update(float dt)
    {
        if (flightValues.GetAltitude() > flightValues.CruiseAltitude)
        {
            UpdateState();
        }

        // TODO - If player gets closer to airport, switch to Descent state
    }

    public override void InitState()
    {
        EnteredNewState(stateManager, State.Climb);
        UpdateInteractable();
    }

    public override void UpdateState()
    {
        stateManager.SwitchState(State.Cruise);
    }
}
