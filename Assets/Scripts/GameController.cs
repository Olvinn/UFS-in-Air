using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public static Dictionary<int, Player> players = new Dictionary<int, Player>();


    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;

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
    public void SpawnPlayer(int _id, string _username, Vector3 _position)
    {
        GameObject _player;
        if (_id == Client.instance.id)
        {
            _player = Instantiate(localPlayerPrefab, _position, new Quaternion());
        }
        else
        {
            _player = Instantiate(playerPrefab, _position, new Quaternion());
        }

        _player.GetComponent<Player>().SetUp(_id, _username);
        players.Add(_id, _player.GetComponent<Player>());
    }

    public void RemovePlayer(int _id)
    {
        if (players.ContainsKey(_id))
        {
            Destroy(players[_id].gameObject);
            players.Remove(_id);
        }
    }

    public void SynchPlayerPos(int id, Vector3 position, Vector3 velocity)
    {
        if (players.ContainsKey(id))
        {
            players[id].transform.position = position;
            players[id].velocity = velocity;
        }
    }

    public void SynchPlayerStats(int id, bool isUFS, bool stunned, bool killed)
    {
        if (players.ContainsKey(id))
        {
            players[id].isUFS = isUFS;
            players[id].stunned = stunned;
            players[id].killed = killed;
        }
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }
}
