using UnityEngine;

public class MoveCommand : ICommand {
    
    readonly Player player;
    readonly float speed = 3.0f;
    
    public MoveCommand(Player player) {
        this.player = player;
    }
        
    public void Execute(Vector2 delta) {
        Move(delta);
    }

    public void Undo() {
    }

    void Move(Vector2 delta) {
        Debug.Log($"move : {delta}");
        player.transform.Translate(delta * Time.deltaTime * speed);
    }
}
