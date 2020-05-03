using UnityEngine;

interface ICommand {
    void Execute(Vector2 delta);
    void Undo();
}