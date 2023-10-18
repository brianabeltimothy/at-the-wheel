using TMPro;
using UnityEngine;

public class HudManager : MonoBehaviour
{
    public PlayerController playerControllerScript;
    public TextMeshProUGUI speedDisplayer;

    void Start()
    {
        speedDisplayer = GameObject.Find("SpeedometerText").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        float playerSpeed = Mathf.Round(playerControllerScript.currentSpeed) * 3;

        if (playerSpeed < 0)
        {
            playerSpeed *= -1.0f;
            speedDisplayer.text = playerSpeed.ToString();
        }
        else
        {
            speedDisplayer.text = playerSpeed.ToString();
        }
    }
}
