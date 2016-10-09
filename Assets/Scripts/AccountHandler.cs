﻿using UnityEngine;
using UnityEngine.UI;
using MMOServer;
using System;

public class AccountHandler : MonoBehaviour {
    private GameObject cursor;
    private MenuHandler menuHandler;


    public void SubmitAccount(string findUser, string findPass, MenuLink ml, bool registering)
    {
        GameObject passwordGameObj = GameObject.Find(findPass);
        InputField passwordInput = passwordGameObj.GetComponent<InputField>();

        GameObject userGameObj = GameObject.Find(findUser);
        InputField usernameInput = userGameObj.GetComponent<InputField>();

        GameObject subObj = ml.GetMenuItem();
        string password = passwordInput.text;
        string userName = usernameInput.text;

        menuHandler.OpenStatusBox(Menus.CharacterMenu);

        CheckInputs(userName, password);
        AccountPacket ap = new AccountPacket();
        byte[] data = ap.GetDataBytes(userName, password);


        SubPacket subPacket = new SubPacket(registering, (ushort)userName.Length, (ushort)password.Length, 0, 0, data, SubPacketTypes.Account);

        BasePacket packetToSend = BasePacket.CreatePacket(subPacket, false, false);

        PacketProcessor.LoginOrRegister(packetToSend);





    }

    private void CheckInputs(string userName, string password)
    {
        if (password.Contains(" ") || userName.Contains(" "))
        {
            throw new Exception("Invalid character in Username or Password");
        }
        if (password == null && userName == null)
        {
            throw new Exception("Empty username or password");
        }
        if (password.Length < 4 || userName.Length < 3)
        {
            throw new Exception("Password and Username length must be greater than 4 characters");
        }
    }
    // Use this for initialization
    void Start () {
        menuHandler = CursorInput.menuHandler;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}