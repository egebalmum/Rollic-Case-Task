using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartMobile : PlayerAbility
{
    public override void NormalUpdate()
    {
        if (Input.touchCount>0)
        {
            GameManager.Instance.SetGameState(GameManager.GameState.Running);
        }
    }
}
