using TMPro;
using UnityEngine;

public class HudManager : MonoBehaviour
{
    public PlayerController playerControllerScript;
    public TextMeshProUGUI speedDisplayer;

    void Start()
    {
        speedDisplayer = GameObject.Find("Speedometer").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        float playerSpeed = Mathf.Floor(playerControllerScript.currentSpeed) * 3;

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
