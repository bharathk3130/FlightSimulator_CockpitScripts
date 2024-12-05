using System.Threading;
using UnityEngine;

public class CruiseStateFSM : FlightFSM
{
    Rigidbody planeRb;
    float autopilotDuration = 5;

    CountDownTimer autoPilotCountdown;

    bool startedCountDown;

    public CruiseStateFSM(FlightStateManager stateManager, State state, FlightValuesSO flightValues, Rigidbody rb) : base(stateManager, state, flightValues)
    {
        planeRb = rb;
    }

    public override void Update(float dt)
    {
        // If your altitude lowers before you press autopilot, it goes back to climb state and tells you to go back up into cruise
        if (flightValues.GetAltitude() < flightValues.AltitudeBeforeSlippingDownToClimb)
        {
            stateManager.SwitchState(State.Climb);
        }

        // After finishing autopilot, call UpdateState()
        if (startedCountDown)
            autoPilotCountdown.Tick(dt);
    }

    public override void InitState()
    {
        EnteredNewState(stateManager, State.Cruise);
        UpdateInteractable();

        autoPilotCountdown = new CountDownTimer(autopilotDuration);
        autoPilotCountdown.OnTimerStart += () => startedCountDown = true;
        autoPilotCountdown.OnTimerStop += UpdateState;

        // Leave it commented. If uncommented, it will automatically go to Descent state after 5 seconds even if you don't press autopilot
        //autoPilotCountdown.Start();
    }

    public override void UpdateState()
    {
        stateManager.SwitchState(State.Descent);
    }

    protected override void FinishedStateInteractions()
    {
        // After autopilot is done, this gets called. Go to next state after 5 seconds
        autoPilotCountdown.Start();
        UpdatePromptUI("");
    }
}
