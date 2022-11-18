using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;

public class PlayerConnectLevel : PlayerAbility
{
    [SerializeField] private float speed;
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
            transform.Translate(directionVector.normalized*speed*Time.deltaTime);
        }
        
    }
}
