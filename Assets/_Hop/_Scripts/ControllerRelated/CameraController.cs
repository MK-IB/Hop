using System.Collections;
using _Hop._Scripts.ControllerRelated;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject followCam, startCam;
    void Start()
    {
        StartCoroutine(OnGameStartCameraChange());
    }
    private void OnEnable()
    {
        MainController.GameStateChanged += GameManager_GameStateChanged;
    }
    private void OnDisable()
    {
        MainController.GameStateChanged -= GameManager_GameStateChanged;
    }
    void GameManager_GameStateChanged(GameState newState, GameState oldState)
    {
        if (newState == GameState.Levelfail)
        {
            OnGameOver();
        }
    }

    IEnumerator OnGameStartCameraChange()
    {
        yield return new WaitForSeconds(.5f);
        followCam.SetActive(true);
        startCam.SetActive(false);
    }

    void OnGameOver()
    {
        followCam.GetComponent<CinemachineCamera>().Follow = null;
        followCam.transform.DORotate(Vector3.right * 55, 1.5f);
    }
}
