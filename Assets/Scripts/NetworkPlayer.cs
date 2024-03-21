using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    private ChangeDetector _changes;
    public TextMeshProUGUI playerName;
    public static NetworkPlayer Local {  get; private set; }
    [SerializeField] private GameObject Camera;

    [Networked, OnChangedRender(nameof(OnNickNameChanged))]
    public NetworkString<_16> nickName { get; private set; }
    private void Awake()
    {
        _changes = GetChangeDetector(ChangeDetector.Source.SimulationState);
    }
    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;
            Camera.gameObject.transform.parent = null;

            RPC_SetNickName(MainMenuHandler.test);
            PlayerPrefs.SetString("PlayerNickname", MainMenuHandler.test);
            PlayerPrefs.Save();

            playerName.text = nickName.ToString();
        }
        else
        {
            Camera.gameObject.SetActive(false);
            OnNickNameChanged();
        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        if(player == Object.InputAuthority)
        {
            Runner.Despawn(Object);
        }
    }

    public override void Render()
    {
        foreach (var change in _changes.DetectChanges(this, out var previousBuffer, out var currentBuffer))
        {
            switch (change)
            {
                case nameof(nickName):
                    var reader = GetPropertyReader<int>(nameof(nickName));
                    var (previous, current) = reader.Read(previousBuffer, currentBuffer);
                    OnStateChanged(previous, current);

                    break;
            }
        }
    }

    private void OnStateChanged(int oldValue, int value)
    {
        playerName.text = nickName.ToString();
        OnNickNameChanged();
    }

    private void OnNickNameChanged()
    {
        playerName.text = nickName.ToString();
        RPC_SetNickName(MainMenuHandler.test);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetNickName(string newNickName, RpcInfo info = default)
    {
        nickName = newNickName;

        RPC_RelayMessage(newNickName, info.Source);
    }


    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_RelayMessage(string newNickName, PlayerRef messageSource)
    {
        nickName = newNickName;
    }

}
