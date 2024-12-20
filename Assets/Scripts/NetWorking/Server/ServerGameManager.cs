using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetWorking.Server.Services;
using NetWorking.Shared;
using Unity.Netcode;
using Unity.Services.Matchmaker.Models;
using UnityEngine;

namespace NetWorking.Server
{
    public class ServerGameManager : IDisposable
    {
        private string serverIP;
        private int serverPort;
        private int queryPort;
        private MatchplayBackfiller backfiller;
        private MultiplayAllocationService multiplayAllocationService;
        
        private Dictionary<string, int> teamIDToTeamIndex = new Dictionary<string, int>();
        
        public NetworkServer NetworkServer { get; private set; }


        public ServerGameManager(string serverIP, int serverPort, int queryPort, NetworkManager manager, NetworkObject playerPrefab)
        {
            this.serverIP = serverIP;
            this.serverPort = serverPort;
            this.queryPort = queryPort;
            NetworkServer = new NetworkServer(manager, playerPrefab);
            multiplayAllocationService = new MultiplayAllocationService();
        }
        
        public async Task StartGameServerAsync()
        {
            await multiplayAllocationService.BeginServerCheck();

            try
            {
                MatchmakingResults matchamkerPayload = await GetMatchmakerPayload();

                if (matchamkerPayload != null)
                {
                    await StaterBackfill(matchamkerPayload);
                    NetworkServer.OnUserJoined += UserJoined;
                    NetworkServer.OnUserLeft += UserLeft;
                }
                else
                {
                    Debug.LogWarning("Matchmaker payload timed out");
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }

            if (!NetworkServer.OpenConnection(serverIP, serverPort))
            {
                Debug.LogWarning("Cannot start game server because network server could not be started.");
                return;
            }
            
        }

        private async Task StaterBackfill(MatchmakingResults payload)
        {
            backfiller = new MatchplayBackfiller($"{serverIP}:{serverPort}", payload.QueueName, payload.MatchProperties, 8);

            if (backfiller.NeedsPlayers())
            {
                await backfiller.BeginBackfilling();
            }
        }

        private void UserJoined(UserData user)
        {
            Team team = backfiller.GetTeamByUserId(user.userAuthId);
            if (!teamIDToTeamIndex.TryGetValue(team.TeamId, out var teamIndex))
            {
                teamIndex = teamIDToTeamIndex.Count;
                teamIDToTeamIndex.Add(team.TeamId, teamIndex);
            }
            
            user.teamIndex = teamIndex;
            
            multiplayAllocationService.AddPlayer();
            if (!backfiller.NeedsPlayers() && backfiller.IsBackfilling)
            {
                _ = backfiller.StopBackfill();
            }
        }
        
        private void UserLeft(UserData user)
        {
            int playerCount = backfiller.RemovePlayerFromMatch(user.userAuthId);
            multiplayAllocationService.RemovePlayer();

            if (playerCount <= 0)
            {
                CloseServer();
                return;
            }

            if (backfiller.NeedsPlayers() && !backfiller.IsBackfilling)
            {
                _ = backfiller.BeginBackfilling();
            }
        }

        private async void CloseServer()
        {
            await backfiller.StopBackfill();
            Dispose();
            Application.Quit();
        }


        

        private async Task<MatchmakingResults> GetMatchmakerPayload()
        {
            Task<MatchmakingResults> matchmakerPayloadTask = multiplayAllocationService.SubscribeAndAwaitMatchmakerAllocation();

            if (await Task.WhenAny(matchmakerPayloadTask, Task.Delay(20000)) == matchmakerPayloadTask)
            {
                return matchmakerPayloadTask.Result;
            }

            return null;
        }

        public void Dispose()
        {
            NetworkServer.OnUserJoined -= UserJoined;
            NetworkServer.OnUserLeft -= UserLeft;
            
            backfiller?.Dispose();
            multiplayAllocationService?.Dispose();
            NetworkServer?.Dispose();
        }
    }
}
