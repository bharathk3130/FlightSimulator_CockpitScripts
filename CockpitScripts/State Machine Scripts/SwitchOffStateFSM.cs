using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOffStateFSM : FlightFSM
{
    Rigidbody planeRb;

    public SwitchOffStateFSM(FlightStateManager stateManager, State state, FlightValuesSO flightValues, Rigidbody rb) : base(stateManager, state, flightValues)
    {
        planeRb = rb;
    }

    public override void InitState()
    {
        EnteredNewState(stateManager, State.SwitchOff);
        UpdateInteractable();
    }

    protected override void FinishedStateInteractions()
    {
        UpdatePromptUI("");
    }
}
