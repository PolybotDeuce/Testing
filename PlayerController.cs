using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public class PlayerController : MonoBehaviour
{
    public bool developerModeOn = false;

    public GameObject shadePrefab;
    public GameObject wendyPrefab;
    public GameObject samPrefab;
    public GameObject teddyPrefab;

    //public GameObject shadeSpawns;
    public Transform[] shadeSpawnPos;

    //public GameObject scoutSpawns;
    public Transform[] scoutSpawnPos;

    private int numScoutsSpawned = 0;

    private List<PlayerMovement> playerMovers;

    private static PlayerController instance;
    public static PlayerController Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        instance = this;
        playerMovers = new List<PlayerMovement>();
    }

    public void SpawnPlayer(string characterName, int playerNum)
    {
        GameObject newObj;
        PlayerMovement tempMov;
        Vector3 spawn = Vector3.zero;

        switch (characterName)
        {
            case "wendy":
                spawn = scoutSpawnPos[numScoutsSpawned].position;
                newObj = (GameObject)Instantiate(wendyPrefab, spawn, Quaternion.identity);
                newObj.name = "Player " + (playerNum + 1);
                newObj.GetComponent<PlayerMovement>().SetPlayer(playerNum, developerModeOn);
                tempMov = newObj.GetComponent<PlayerMovement>();
                playerMovers.Add(tempMov);
                numScoutsSpawned++;
                break;

            case "shade":
                int rand = Random.Range(0, shadeSpawnPos.Length);
                spawn = shadeSpawnPos[rand].position;
                newObj = (GameObject)Instantiate(shadePrefab, spawn, Quaternion.identity);
                newObj.name = "Player " + (playerNum + 1);
                newObj.GetComponent<PlayerMovement>().SetPlayer(playerNum, developerModeOn);
                tempMov = newObj.GetComponent<PlayerMovement>();
                playerMovers.Add(tempMov);
                break;
        }
    }

    public bool DEVTOOLnextPlayer (PlayerMovement currentPlayer)
    {
        //print("next player request");

        bool switchedPlayer = false;
		int currentPlayerNum = currentPlayer.GetPlayerNum();
        int nextPlayerNum = currentPlayerNum + 1;

		while (!switchedPlayer && nextPlayerNum != currentPlayerNum)
        {
            if (nextPlayerNum == playerMovers.Count)
                nextPlayerNum = 0;

			switchedPlayer = DEVTOOLswapPlayer(currentPlayer, playerMovers[nextPlayerNum]);

            nextPlayerNum++;
        }

		return switchedPlayer;
    }

    public bool DEVTOOLprevPlayer(PlayerMovement currentPlayer)
    {
        //print("prev player request");

        bool switchedPlayer = false;
		int currentPlayerNum = currentPlayer.GetPlayerNum();
        int prevPlayerNum = currentPlayerNum - 1;

		while (!switchedPlayer && prevPlayerNum != currentPlayerNum)
        {
            if (prevPlayerNum < 0)
                prevPlayerNum = playerMovers.Count - 1;

			switchedPlayer = DEVTOOLswapPlayer(currentPlayer, playerMovers[prevPlayerNum]);

            prevPlayerNum--;
        }

		return switchedPlayer;
    }

	private bool DEVTOOLswapPlayer(PlayerMovement currentPlayer, PlayerMovement otherPlayer)
	{
		if (!otherPlayer.isControllerConnected() && !otherPlayer.getIsActive())
		{
			PlayerIndex temp = otherPlayer.getPlayerIndex();
			otherPlayer.setPlayerIndex(currentPlayer.getPlayerIndex());
			otherPlayer.setIsActive(true);
			currentPlayer.setPlayerIndex(temp);
			currentPlayer.setIsActive(false);
			GameController.Instance.DEVTOOLsetSwap();
			return true;
		}
		return false;
	}
}