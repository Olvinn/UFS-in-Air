using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public static Dictionary<int, Player> players = new Dictionary<int, Player>();
    int botInd = 0;

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    public GameObject botPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    /// <summary>Spawns a player.</summary>
    /// <param name="_id">The player's ID.</param>
    /// <param name="_name">The player's name.</param>
    /// <param name="_position">The player's starting position.</param>
    /// <param name="_rotation">The player's starting rotation.</param>
    public void SpawnPlayer(int _id, Vector3 _position, bool isBot)
    {
        GameObject _player;
        if (_id == Client.instance.id)
        {
            _player = Instantiate(localPlayerPrefab, _position, new Quaternion());
        }
        else if (isBot)
        {
            _player = Instantiate(botPrefab, _position, new Quaternion());
        }
        else
        {
            _player = Instantiate(playerPrefab, _position, new Quaternion());
        }

        players[_id] = _player.GetComponent<Player>();

        _player.GetComponent<Player>().SetUp(_id);
    }

    public void AddPlayer(int _id)
    {
        if (!players.ContainsKey(_id))
            players.Add(_id, null);
    }

    public void Clear(int _id)
    {
        foreach (int id in players.Keys)
            if (players[id])
                Destroy(players[id].gameObject);

        players.Clear();
    }

    public void RemovePlayer(int _id)
    {
        if (players.ContainsKey(_id))
        {
            if (players[_id])
                Destroy(players[_id].gameObject);
            players.Remove(_id);
        }
    }

    public void SynchPlayerPos(int id, Vector3 position, Vector3 velocity)
    {
        if (players.ContainsKey(id))
        {
            if (players[id])
            {
                bool me = id == Client.instance.id;
                bool myBot = players[id].isBot && Client.instance.isHost;

                if (me || myBot)
                    return;

                players[id].transform.position = position;
                players[id].velocity = velocity;
            }
        }
    }

    public void SynchPlayerStats(int id, bool isUFS, bool stunned, bool killed, bool isBot)
    {
        if (players.ContainsKey(id) && players[id])
        {
            players[id].isUFS = isUFS;
            players[id].stunned = stunned;
            players[id].killed = killed;
            players[id].isBot = isBot;
        }
    }

    public void SendLoadGame()
    {
        Client.instance.StartLoadGame();
    }

    public void ConnectToRoom()
    {
        Client.instance.ConnectToRoom();
    }

    public void DisconnectFromRoom()
    {
        Client.instance.DisconnectToRoom();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OnStartLoadGameLevel()
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync("Game");
        StartCoroutine(SpawnPlayers(loading));
    }

    public int AliveCount()
    {
        int i = 0;
        foreach (int id in players.Keys)
            if (players[id])
                if (!players[id].isUFS && !players[id].isBot) i++;
        return i;
    }

    public int UFSCount()
    {
        int i = 0;
        foreach (int id in players.Keys)
            if (players[id])
                if (players[id].isUFS && !players[id].isBot) i++;
        return i;
    }

    IEnumerator SpawnPlayers(AsyncOperation loading)
    {
        while (!loading.isDone)
            yield return new WaitForSeconds(1);

        Client.instance.SendReadyForPlay();
    }
}
