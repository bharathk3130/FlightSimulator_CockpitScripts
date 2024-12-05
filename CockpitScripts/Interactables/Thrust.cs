using UnityEngine;

public class Thrust : MonoBehaviour
{
    [SerializeField] bool debugMode;
    [HideInInspector] public Observer<float> ThrustPercent = new(0);
    [SerializeField] float thrustPercent = 0;

    PokeLever thrustLever;

    private void Awake()
    {
        thrustLever = GetComponent<PokeLever>();
    }

    private void Start()
    {
        thrustLever.OnLeverMove += OnThrustLeverMove;
    }

    private void Update()
    {
        if (debugMode)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ThrustPercent.Value = 1;
                thrustPercent = 1;
            }
        }
    }

    void OnThrustLeverMove()
    {
        ThrustPercent.Value = thrustLever.GetThrottlePercentage();
    }

    private void OnValidate()
    {
        if (thrustPercent != ThrustPercent.Value)
        {
            ThrustPercent.Value = thrustPercent;
        }
    }
}
