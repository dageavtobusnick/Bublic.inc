using UnityEngine;

public class ExitController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || collision.isTrigger) return;

        var currentRoom = gameObject.GetComponentInParent<RoomProperties>();
        GameController.CurrentRoom = currentRoom;
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || collision.isTrigger) return;

        var currentRoom = gameObject.GetComponentInParent<RoomProperties>();
        currentRoom.CloseExits();
        var ai = currentRoom.GetComponentsInChildren<IAttacker>();
        if (ai != null)
            if (!currentRoom.IsCleared)
                foreach (var x in ai) x.Attack();
            else
                foreach (var x in ai) x.StopAttack();
    }
}
