using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour
{
    public GameObject loadScreen;
    public GameObject gameUI;
    public GameObject roomManager;
    public Slider slider;
    public Text text;
    public float totalRoomNeed;
    private int previousRoomGenerate = 0;
    private void Start()
    {
        if (roomManager)
        {
            Cursor.visible = false;
            StartCoroutine(LoadScene());
        }

    }
    public void BackToMenu()
    {
        StartCoroutine(LoadMenu());
    }
    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel());
    }
    IEnumerator LoadMenu()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex - 1);
        while (!operation.isDone)
        {
            yield return null;
        }
    }
    IEnumerator LoadLevel()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        while (!operation.isDone)
        {
            slider.value = operation.progress;
            text.text = operation.progress * 100 + "%" ;
            yield return null;
        }
    }

    IEnumerator LoadScene()
    {
        while (!roomManager.GetComponent<RoomManager>().hasFinishInitialize)
        {
            if (!roomManager.GetComponent<RoomManager>().hasGenerateRooms)
            {
                text.text = "Prepareing Room Layout...";
            }
            else
            {
                if (!roomManager.GetComponent<RoomManager>().hasGeneratedItems)
                {
                    text.text = "Preparing items you can use...";
                }
                else
                {
                    if (!roomManager.GetComponent<RoomManager>().hasGeneratedCharacter)
                    {
                        text.text = "Prepareing to escape...";
                    }
                }
            }
            if(roomManager.GetComponent<RoomManager>().currentTotalRoomNum > previousRoomGenerate)
            {
                Debug.Log("add progress");
                Debug.Log(roomManager.GetComponent<RoomManager>().currentTotalRoomNum / totalRoomNeed);
                slider.value = roomManager.GetComponent<RoomManager>().currentTotalRoomNum/totalRoomNeed;
            }
            previousRoomGenerate = Mathf.Max(roomManager.GetComponent<RoomManager>().currentTotalRoomNum, previousRoomGenerate);
            yield return null;
        }
        if (roomManager.GetComponent<RoomManager>().hasFinishInitialize)
        {
            slider.value = 1f;
            yield return null;
        }
        loadScreen.SetActive(false);
        gameUI.SetActive(true);
    }

}
