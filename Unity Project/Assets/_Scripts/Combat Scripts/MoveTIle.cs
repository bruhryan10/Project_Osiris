using UnityEngine;

public class MoveTIle : MonoBehaviour
{
    [SerializeField] TurnManager turnM;
    [SerializeField] CombatManager combatM;
    [SerializeField] TileMovement playerTiles;
    [SerializeField] CombatRooms rooms;

    private void Start()
    {
        turnM = GameObject.Find("CombatManager").GetComponent<TurnManager>();
        combatM = GameObject.Find("CombatManager").GetComponent<CombatManager>();
        rooms = GameObject.Find("CombatManager").GetComponent<CombatRooms>();
    }
    void Update()
    {
        if (!combatM.CheckCombatState())
            return;
        if (turnM.GetCurrentTurn().name == "Support Ally" || turnM.GetCurrentTurn().name == "Tank Ally")
            Ally();
        if (turnM.GetCurrentTurn().name == "Player")
            Player();
    }
    void Ally()
    {
        playerTiles = turnM.GetCurrentTurn().GetComponent<TileMovement>();
        int playerX = playerTiles.CheckMoveX();
        int playerY = playerTiles.CheckMoveY();
        string north = rooms.activeArray[17 - playerY, playerX - 1];
        string south = rooms.activeArray[17 - playerY, playerX + 1];
        string east = rooms.activeArray[17 - playerY - 1, playerX];
        string west = rooms.activeArray[17 - playerY + 1, playerX];
        if (this.name == "north" && (north == "|" || north == "e" || north == "p" || north[0] == 'a' && north[1] != playerTiles.allyNum[1]))
            this.GetComponent<SpriteRenderer>().enabled = false;
        else if (this.name == "north")
            this.GetComponent<SpriteRenderer>().enabled = true;

        if (this.name == "south" && (south == "|" || south == "e" || south == "p" || south[0] == 'a' && south[1] != playerTiles.allyNum[1]))
            this.GetComponent<SpriteRenderer>().enabled = false;
        else if (this.name == "south")
            this.GetComponent<SpriteRenderer>().enabled = true;

        if (this.name == "west" && (west == "|" || west == "e" || west == "p" || west[0] == 'a' && west[1] != playerTiles.allyNum[1]))
            this.GetComponent<SpriteRenderer>().enabled = false;
        else if (this.name == "west")
            this.GetComponent<SpriteRenderer>().enabled = true;

        if (this.name == "east" && (east == "|" || east == "e" || east == "p" || east[0] == 'a' && east[1] != playerTiles.allyNum[1]))
            this.GetComponent<SpriteRenderer>().enabled = false;
        else if (this.name == "east")
            this.GetComponent<SpriteRenderer>().enabled = true;
    }
    void Player()
    {
        playerTiles = turnM.GetCurrentTurn().GetComponent<TileMovement>();
        int playerX = playerTiles.CheckMoveX();
        int playerY = playerTiles.CheckMoveY();
        string south = rooms.activeArray[17 - playerY, playerX - 1];
        string north = rooms.activeArray[17 - playerY, playerX + 1];
        string west = rooms.activeArray[17 - playerY - 1, playerX];
        string east = rooms.activeArray[17 - playerY + 1, playerX];
        if (this.name == "north" && (north == "|" || north == "e" || north[0] == 'a'))
            this.GetComponent<SpriteRenderer>().enabled = false;
        else if (this.name == "north")
            this.GetComponent<SpriteRenderer>().enabled = true;

        if (this.name == "south" && (south == "|" || south == "e" || south[0] == 'a'))
            this.GetComponent<SpriteRenderer>().enabled = false;
        else if (this.name == "south")
            this.GetComponent<SpriteRenderer>().enabled = true;

        if (this.name == "west" && (west == "|" || west == "e" || west[0] == 'a'))
            this.GetComponent<SpriteRenderer>().enabled = false;
        else if (this.name == "west")
            this.GetComponent<SpriteRenderer>().enabled = true;

        if (this.name == "east" && (east == "|" || east == "e" || east[0] == 'a'))
            this.GetComponent<SpriteRenderer>().enabled = false;
        else if (this.name == "east")
            this.GetComponent<SpriteRenderer>().enabled = true;
    }
}
