using System.Collections;
using System.Collections.Generic;
using PlayerController.InputController;
using UnityEngine;

public class CharacterHandlerController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void EnabledInputController()
    {
        PlayerInputController playerInputController = GetComponent<PlayerInputController>();
        playerInputController.enabled = true;
    }

    public void DisablePlayerInputController()
    {
        PlayerInputController playerInputController = GetComponent<PlayerInputController>();
        playerInputController.enabled = false;
    }
}
