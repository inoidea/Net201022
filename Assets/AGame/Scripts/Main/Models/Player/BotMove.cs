using UnityEngine;

public class BotMove : PlayerMove
{
    void Update()
    {
        if (!photonView.IsMine)
            return;

        
    }
}
