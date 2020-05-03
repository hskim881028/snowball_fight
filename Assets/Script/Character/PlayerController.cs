using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : Controller {
    [SerializeField] Player playerPrefab;
    [SerializeField] InputHandler inputHandler;
    Player player;
    
    public override void OnAwake() {
        player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        inputHandler.Init(new MoveCommand(player), new AttackCommand(player));
    }

    public override void OnStart() {
    }

    public override void Attack() {
    }

    public override void Move() {
    }
}
