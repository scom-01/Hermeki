using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkingManager : MonoBehaviourPunCallbacks
{
    public TMP_Text StatusText;
    public TMP_InputField roomInput, NickNameInput;

    private void Awake()
    {
        Screen.SetResolution(1920, 1080, false);
    }

    private void Update()
    {
        StatusText.text = PhotonNetwork.NetworkClientState.ToString();
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();
    public void Disconnect() => PhotonNetwork.Disconnect();
    public void JoinLobby() => PhotonNetwork.JoinLobby();

    public void CreateRoom() => PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 4 });
    public void JoinRoom() => PhotonNetwork.JoinRoom(roomInput.text);
    public void JoinOrCreateRoom() => PhotonNetwork.JoinOrCreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 4 }, null);
    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();
    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    #region Override
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("서버 접속 완료");
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
    }
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("방 생성 완료");
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("Join Room");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        print("서버 접속 해제");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("Create Room Failed");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("Join Room Failed");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        print("Join Random Room Failed");
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("Join Lobby");
    }
    #endregion

    [ContextMenu("Info")]
    void Info()
    {
        if(PhotonNetwork.InRoom)
        {
            print($"현재 방 이름 :  {PhotonNetwork.CurrentRoom.Name}");
            print($"현재 방 인원 수 :  {PhotonNetwork.CurrentRoom.PlayerCount}");
            print($"현재 방 최대 인원 수 :  {PhotonNetwork.CurrentRoom.MaxPlayers}");

            string playerstr = "방에 있는 플레이어 목록 : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                playerstr += PhotonNetwork.PlayerList[i].NickName + ", ";
            }
            print(playerstr);
        }
        else
        {
            print($"접속한 인원 수 :  {PhotonNetwork.CountOfPlayers}");
            print($"방 개수 :  {PhotonNetwork.CountOfRooms}");
            print($"모든 방에 있는 인원 수 :  {PhotonNetwork.CountOfPlayersInRooms}");
            print($"로비에 있는가? :  {PhotonNetwork.InLobby}");
            print($"현결 상태 :  {PhotonNetwork.IsConnected}");
        }
    }
}
