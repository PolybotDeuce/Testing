using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
                newObj.name = "Player " + playerNum;
                newObj.GetComponent<PlayerMovement>().SetPlayer(playerNum, developerModeOn);
                tempMov = newObj.GetComponent<PlayerMovement>();
                playerMovers.Add(tempMov);
                numScoutsSpawned++;
                break;

            case "shade":
                int rand = Random.Range(0, shadeSpawnPos.Length);
                spawn = shadeSpawnPos[rand].position;
                newObj = (GameObject)Instantiate(shadePrefab, spawn, Quaternion.identity);
                newObj.name = "Player " + playerNum;
                newObj.GetComponent<PlayerMovement>().SetPlayer(playerNum, developerModeOn);
                tempMov = newObj.GetComponent<PlayerMovement>();
                playerMovers.Add(tempMov);
                break;
        }
    }

    public void DEVTOOLnextPlayer (PlayerMovement currentPlayer)
    {
        print("next player request");

        bool switchedPlayer = false;
        int nextPlayerNum = currentPlayer.GetPlayerNum() + 1;

        do
        {
            if (nextPlayerNum > playerMovers.Count)
                nextPlayerNum = 1;

            if (!playerMovers[nextPlayerNum - 1].isControllerConnected())
            {
                int temp = playerMovers[nextPlayerNum - 1].GetPlayerNum();
                playerMovers[nextPlayerNum - 1].SetPlayer(currentPlayer.GetPlayerNum(), developerModeOn);
                currentPlayer.SetPlayer(temp, developerModeOn);
                switchedPlayer = true;
                break;
            }

            nextPlayerNum++;
        }
        while (!switchedPlayer);
    }

    public void DEVTOOLprevPlayer(PlayerMovement currentPlayer)
    {
        print("prev player request");

        bool switchedPlayer = false;
        int prevPlayerNum = currentPlayer.GetPlayerNum() - 1;

        do
        {
            if (prevPlayerNum < 1)
                prevPlayerNum = playerMovers.Count;

            if (!playerMovers[prevPlayerNum - 1].isControllerConnected())
            {
                int temp = playerMovers[prevPlayerNum - 1].GetPlayerNum();
                playerMovers[prevPlayerNum - 1].SetPlayer(currentPlayer.GetPlayerNum(), developerModeOn);
                currentPlayer.SetPlayer(temp, developerModeOn);
                switchedPlayer = true;
                break;
            }

            prevPlayerNum--;
        }
        while (!switchedPlayer);
    }
}
