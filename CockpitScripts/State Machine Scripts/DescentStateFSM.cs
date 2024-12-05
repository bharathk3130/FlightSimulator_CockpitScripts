using UnityEngine;

public class DescentStateFSM : FlightFSM
{
    Rigidbody planeRb;

    public DescentStateFSM(FlightStateManager stateManager, State state, FlightValuesSO flightValues, Rigidbody planeRb) : base(stateManager, state, flightValues)
    {
        this.planeRb = planeRb;
    }

    //public override void InitState()
    //{
    ////    Debug.Log("INSIDE DESCENT STATE");
    ////    currentSubState = DescentSubState.DecreaseAlt;
    ////    stateManager.descentSubStateMap[currentSubState].Initialize();
    ////    Debug.Log("Decrease alt");
    //}

    public override void Update(float dt)
    {
        //if (Weather.GetAltitude(planeRb.position.y) < flightValues.ApproachAltitude)
        if (flightValues.GetAltitude() < flightValues.ApproachAltitude)
        {
            UpdateState();
        }
    }

    public override void InitState()
    {
        EnteredNewState(stateManager, State.Descent);
        UpdateInteractable();
    }

    public override void UpdateState()
    {
        stateManager.SwitchState(State.Approach);
    }

    protected override void FinishedStateInteractions()
    {
        UpdatePromptUI("Decrease altitude");
    }
}