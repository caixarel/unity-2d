using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnimatorEvents : MonoBehaviour
{

    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void animationTrigger()
    {
        player.AttackOver();
    }
}
