using System;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    public PlayerController playerController;
    public DoorController door;
    public DoorController otherDoor;
    public int doorNumber;

    public bool canLeave;

    public float interactionDistance = 0.3f;

    public GameControllerGrassWorld GameControllerGrassWorld;
    public GameControllerPirateShip GameControllerPirateShip;

    public string sceneID;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (doorNumber == 1)
        {
            canLeave = false;
        }
        else {
            canLeave = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneID == "GrassWorld2")
        {
            if (Input.GetKeyDown(KeyCode.F) && (canLeave))
            {
                if (Vector2.Distance(playerController.transform.position, transform.position) < interactionDistance)
                {
                    Debug.Log("within door collider and pressed interact button (F)");
                    GameControllerGrassWorld.updateInside();
                    if (doorNumber == 1)
                    {
                        // move to outside door
                        playerController.transform.position = new Vector3(otherDoor.transform.position.x, otherDoor.transform.position.y - 1, playerController.transform.position.z);
                    }
                    else if (doorNumber == 2)
                    {
                        // move to inside door
                        playerController.transform.position = new Vector3(door.transform.position.x, door.transform.position.y + 1, playerController.transform.position.z);
                    }
                }
            }
        }
        else if (sceneID == "PirateShip3")
        {
            if (Input.GetKeyDown(KeyCode.F) && (canLeave))
            {
                if (Vector2.Distance(playerController.transform.position, transform.position) < interactionDistance)
                {
                    Debug.Log("within door collider and pressed interact button (F)");
                    // GameControllerGrassWorld.updateInside();
                    if (doorNumber == 1)
                    {
                        // move to outside door
                        playerController.transform.position = new Vector3(otherDoor.transform.position.x + 0.5f, otherDoor.transform.position.y - 1, playerController.transform.position.z);
                    }
                    else if (doorNumber == 2)
                    {
                        // move to inside door
                        playerController.transform.position = new Vector3(door.transform.position.x + 1, door.transform.position.y, playerController.transform.position.z);
                    }
                }
            }
        }
        }

    public void setCanLeave(bool status)
    {
        canLeave = status;
    }
}
