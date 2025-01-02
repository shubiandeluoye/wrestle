using UnityEngine;
using UnityEngine.UI;
using Fusion;
using Assets.Scripts.WJ.Core.Network;

public class WJNetworkUI : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private WJNetworkManager networkManager;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;

    private void Start()
    {
        hostButton.onClick.AddListener(() => {
            networkManager.StartGame(GameMode.Host);
            SwitchPanel(true);
        });
        
        joinButton.onClick.AddListener(() => {
            networkManager.StartGame(GameMode.Client);
            SwitchPanel(true);
        });

        SwitchPanel(false);
    }

    private void SwitchPanel(bool inGame)
    {
        menuPanel.SetActive(!inGame);
        gamePanel.SetActive(inGame);
    }
} 