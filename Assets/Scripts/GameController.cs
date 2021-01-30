using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void SynchPlayerPos(int _id, Vector3 _position)
    {
        if (players.ContainsKey(_id))
        {
            players[_id].transform.position = _position;
        }
    }
}
