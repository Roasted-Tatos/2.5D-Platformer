using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPanel : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer _lightColor;
    [SerializeField]
    private int _requiredPoints = 1;
    [SerializeField]
    private GameObject _terminalSign;
    [SerializeField]
    private GameObject _callingElevatorSign;
    [SerializeField]
    private bool _calledElevator = false;


    private Player player;

    private ElevatorLift elevatorLift;
    // Start is called before the first frame update
    void Start()
    {
        elevatorLift = GameObject.Find("Elevator_Platform").GetComponent<ElevatorLift>();
        if(elevatorLift ==null)
        {
            Debug.Log("Elevator Platform not accessed");
        }
        player = GameObject.Find("Player").GetComponent<Player>();
        
        _callingElevatorSign.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && _calledElevator == false)
        {
            if (player.totalPoints() >= _requiredPoints)
            {
                    elevatorLift.CallElevator();
                    _lightColor.material.color = Color.green;
                _terminalSign.SetActive(false);
                _callingElevatorSign.SetActive(true);
                _calledElevator = true;
            } 
        }
    }
}
