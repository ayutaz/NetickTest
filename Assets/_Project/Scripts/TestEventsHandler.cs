using Netick;
using Netick.Samples.FPS;
using UnityEngine;

namespace _Project
{
    public class TestEventsHandler : NetworkEventsListner
    {
        [SerializeField] private GameObject playerPrefab;

        public override void OnInput(NetworkSandbox sandbox)
        {
            var input = sandbox.GetInput<TestInput>();

            input.Movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            input.IsJump = Input.GetKeyDown(KeyCode.Space);
        }

        public override void OnSceneLoaded(NetworkSandbox sandbox)
        {
            if (sandbox.IsClient) return;

            for (int i = 0; i < sandbox.ConnectedPlayers.Count; i++)
            {
                // if SpawnPlayerForHost is set to false, we don't spawn a player for the server
                // index zero is the server player

                if (i == 0) continue;

                var p = sandbox.ConnectedPlayers[i];

                var spawnPos = Vector3.up * 10f + Vector3.left * (i);
                var player = sandbox.NetworkInstantiate(playerPrefab, spawnPos, Quaternion.identity, p).GetComponent<FPSController>();
                p.PlayerObject = player.gameObject;
            }
        }

        public override void OnClientConnected(NetworkSandbox sandbox, NetworkConnection client)
        {
            var spawnPos = Vector3.up * 10f + Vector3.left * (1 + sandbox.ConnectedPlayers.Count);
            var player = sandbox.NetworkInstantiate(playerPrefab, spawnPos, Quaternion.identity, client).GetComponent<FPSController>();
            client.PlayerObject = player.gameObject;
        }
    }
}