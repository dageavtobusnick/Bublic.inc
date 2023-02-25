using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static PlayerController Player;
    public static int CoinsCount;
    public static MeleeWeapon CurrentWeapon;
    public static PlayerInputActions PlayerInputActions;

    public static int DamageBonus;
    public static int LuckBonus;

    public static InventoryScript Inventory;

    public static StageGeneration Stage;
    public static RoomProperties CurrentRoom;

    public static int RoomsCleared;

    public static readonly int VirtualTeamId=-2;
    public static readonly int UndeadId = 1;

    private void Awake()
    {
        Player = FindObjectOfType<PlayerController>();
        Stage = FindObjectOfType<StageGeneration>();
        Inventory = FindObjectOfType<InventoryScript>();
        PlayerInputActions = FindObjectOfType<PlayerInputActions>();
    }

    public static void IncreaseClearedRoomsCount()
    {
        RoomsCleared++;
        if (RoomsCleared == Stage.BossSpawnDelay)
            Stage.GenerateBossRoom();
    }

    public static void ReloadWorld()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
