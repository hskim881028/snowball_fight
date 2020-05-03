using UnityEngine;

public class AttackCommand : ICommand {

   public AttackCommand(Player player) {
   }
    
    public void Execute(Vector2 delta) {
        Attack();
    }

    public void Undo() {
    }

    void Attack() {
    }
}
