using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConnectLevel : PlayerAbility
{
    public override void NormalUpdate()
    {
        var directionVector = GameManager.Instance.curLevel.transform.position - transform.position;
        if (directionVector.magnitude <= 0.1f)
        {
            transform.position = GameManager.Instance.curLevel.transform.position;
            GameManager.Instance.SetGameState(GameManager.GameState.Rest);
        }
        else
        {
            transform.Translate(directionVector.normalized*20f*Time.deltaTime);
        }
        
    }
}
