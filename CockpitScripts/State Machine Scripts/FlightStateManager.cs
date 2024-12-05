using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum State
{
    Start,
    TakeOff,
    Climb,
    Cruise,
    Descent,
    Approach,
    Landing,
    SwitchOff
}

public class FlightStateManager : MonoBehaviour
{

    public TextMeshProUGUI prompts;

    FlightFSM currentStateScript;

    StartStateFSM StartState;
    TakeOffStateFSM TakeOffState;
    ClimbStateFSM ClimbState;
    CruiseStateFSM CruiseState;
    DescentStateFSM DescentState;
    ApproachStateFSM ApproachState;
    LandingStateFSM LandingState;
    SwitchOffStateFSM SwitchOffState;

    public Dictionary<State, FlightFSM> stateMap;

    [SerializeField] FlightValuesSO flightValues;
    [SerializeField] Rigidbody planeRb;

    [Header("Debugging")]
    [SerializeField] State currentState;

    State currentStateBackup;

    public static Observer<State> CurrentState { get; private set; } = new(State.Start);

    void Start()
    {
        InitializeStateScripts();
        InitializeDictionary();
        SwitchState(State.Start);

        flightValues.Initialize(planeRb);
    }

    void Update()
    {
        currentStateScript.Update(Time.deltaTime);
    }

    void InitializeStateScripts()
    {
        StartState = new StartStateFSM(this, State.Start, flightValues, planeRb);
        TakeOffState = new TakeOffStateFSM(this, State.TakeOff, flightValues, planeRb);
        ClimbState = new ClimbStateFSM(this, State.Climb, flightValues, planeRb);
        CruiseState = new CruiseStateFSM(this, State.Cruise, flightValues, planeRb);
        DescentState = new DescentStateFSM(this, State.Descent, flightValues, planeRb);
        ApproachState = new ApproachStateFSM(this, State.Approach, flightValues, planeRb);
        LandingState = new LandingStateFSM(this, State.Landing, flightValues, planeRb);
        SwitchOffState = new SwitchOffStateFSM(this, State.SwitchOff, flightValues, planeRb);
    }

    void InitializeDictionary()
    {
        stateMap = new Dictionary<State, FlightFSM>();

        stateMap[State.Start] = StartState;
        stateMap[State.TakeOff] = TakeOffState;
        stateMap[State.Climb] = ClimbState;
        stateMap[State.Cruise] = CruiseState;
        stateMap[State.Descent] = DescentState;
        stateMap[State.Approach] = ApproachState;
        stateMap[State.Landing] = LandingState;
        stateMap[State.SwitchOff] = SwitchOffState;
    }


    public void SwitchState(State nextState)
    {
        currentState = nextState;
        currentStateBackup = currentState;
        CurrentState.Value = currentState;

        currentStateScript = stateMap[currentState];

        if (currentStateScript != null)
            currentStateScript.InitState();
    }

    private void OnValidate()
    {
        if (currentState != currentStateBackup)
        {
            // Switch to the state if it's been changed in the inspector
            SwitchState(currentState);
        }
    }

    [ContextMenu("Update State")]
    public void Debug_UpdateState()
    {
        currentStateScript.UpdateState();
    }
}
