using UnityEngine;
using Unity.Netcode;

public class NetworkConnect : MonoBehaviour
{
    [SerializeField] private TennisManager tennisManager;
    [SerializeField] private GameObject createButton;
    [SerializeField] private GameObject joinButton;
    [SerializeField] private GameObject startButton;
    public void Create()
    {
        NetworkManager.Singleton.StartHost();
        startButton.SetActive(true);
        HideButtons();
        Debug.Log("j");
    }

    public void Join()
    {
        NetworkManager.Singleton.StartClient();
        HideButtons();
    }

    public void HideButtons()
    {
        createButton.SetActive(false);
        joinButton.SetActive(false);
    }

    public void StartMatch()
    {
        tennisManager.StartMatch(NetworkManager.Singleton.ConnectedClientsIds.Count);
        startButton.SetActive(false);
    }
}
