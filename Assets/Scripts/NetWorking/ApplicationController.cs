using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetWorking.Client;
using NetWorking.Host;
using NetWorking.Server;
using NetWorking.Shared;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NetWorking
{
    public class ApplicationController : MonoBehaviour
    {
        [SerializeField] private ClientSingleton clientPrefab;
        [SerializeField] private HostSingleton hostPrefab;
        [SerializeField] private NetworkObject playerPrefab;

        private ApplicationData appData;
        
        private const string GAME_SCENE_NAME = "Game";

    
        private async void Start()
        {
            DontDestroyOnLoad(gameObject);

            await LaunchInMode(SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null);


        }

        private async Task LaunchInMode(bool isDedicatedServer)
        {
            {
                HostSingleton hostSingleton = Instantiate((hostPrefab));
                hostSingleton.CreateHost(playerPrefab);

                ClientSingleton clientSingleton = Instantiate(clientPrefab);
                bool authenticated = await clientSingleton.CreateClient();

           
                if (authenticated)
                {
                    clientSingleton.GameManager.GoToMenu();
                }
            }
        }
    }
}
