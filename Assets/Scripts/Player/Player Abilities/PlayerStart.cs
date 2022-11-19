using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStart : PlayerAbility
{
    public override void NormalUpdate()
    {
        if (Input.anyKey)
        {
            GameManager.Instance.SetGameState(GameManager.GameState.Running);
        }
    }
}
