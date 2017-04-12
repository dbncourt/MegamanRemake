using UnityEngine;
using PlayerController.InputController;
using System.Collections;

public class CharacterMovementController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        InputPlayerController playerController = GetComponent<InputPlayerController>();
        if (playerController != null)
        {
            playerController.BindAxis(AxisInputConstants.HORIZONTAL, Run);
        }
        else
        {
            Debug.LogError("InputPlayerController not found");
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Run(float value)
    {
        value *= Time.deltaTime;
        transform.Translate(value, 0.0f, 0.0f);
    }
}
