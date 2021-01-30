using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    public void StartButton()
    {
        Application.LoadLevel(4);
    }

    public void BackButton()
    {
        Application.LoadLevel(0);
    }

    public static void SetPlayersNumber(int curNum, int maxNum)
    {

    }

    private RectTransform _transform;

    private void Start()
    {
        _transform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        _transform.localScale += new Vector3(-1.0f * Time.deltaTime, 1.0f, 1.0f);

    }
}
