using System.Numerics;
using UnityEngine;

public class ApproachStateFSM : FlightFSM
{
    Rigidbody planeRb;

    public ApproachStateFSM(FlightStateManager stateManager, State state, FlightValuesSO flightValues, Rigidbody rb) : base(stateManager, state, flightValues)
    {
        planeRb = rb;
    }
    //public override void InitState()
    //{
    ////    Debug.Log("INSIDE APPROACH STATE");
    ////    currentSubState = ApproachSubState.ApproachRunway;
    ////    stateManager.approachSubStateMap[currentSubState].Initialize();
    ////    Debug.Log("Align with runway");
    //}

    public override void Update(float dt)
    {
        //if (Weather.GetAltitude(planeRb.position.y) < flightValues.LandAltitude)
        if (flightValues.GetAltitude() < flightValues.LandAltitude)
        {
            UpdateState();
        }
    }

    public override void InitState()
    {
        EnteredNewState(stateManager, State.Approach);
        UpdateInteractable();
    }

    public override void UpdateState()
    {
        stateManager.SwitchState(State.Landing);
    }

    protected override void FinishedStateInteractions()
    {
        UpdatePromptUI("Decrease altitude");
    }
}
