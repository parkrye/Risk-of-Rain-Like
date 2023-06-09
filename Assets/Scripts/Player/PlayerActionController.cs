using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionController : MonoBehaviour
{
    PlayerDataModel playerDataModel;

    void Awake()
    {
        playerDataModel = GetComponent<PlayerDataModel>();
    }

    void OnAction1(InputValue inputValue)
    {
        if(playerDataModel.hero.Action1(inputValue.isPressed))
        {

        }
    }

    void OnAction2(InputValue inputValue)
    {
        if (playerDataModel.hero.Action2(inputValue.isPressed))
        {

        }
    }

    void OnAction3(InputValue inputValue)
    {
        if (playerDataModel.hero.Action3(inputValue.isPressed))
        {

        }
    }

    void OnAction4(InputValue inputValue)
    {
        if (playerDataModel.hero.Action4(inputValue.isPressed))
        {

        }
    }
}
