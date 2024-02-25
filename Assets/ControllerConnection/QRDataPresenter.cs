using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Controllers.Remote;
using Proyecto26;
using UnityEngine;
using UnityEngine.UI;


public class QRDataPresenter : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Image _image;
    private bool _hasSpriteRenderer;
    private bool _hasImage;

    
    
    
    [DllImport("__Internal")]
    private static extern void GetGameQrCode (string gameId);
    private void Start()
    {   
        Debug.Log("Unity GameRemoteSocket Start");
        string id = LobbyManagerSocket.instance.id;
        #if UNITY_WEBGL == true && UNITY_EDITOR == false
                GetGameQrCode(id);
        #else
            Debug.Log("GetGameQrCode: " + id);
        #endif
        Debug.Log("Unity GameRemoteSocket End");

        _spriteRenderer = GetComponent<SpriteRenderer>();

        _image = GetComponent<Image>();

        _hasSpriteRenderer = _spriteRenderer != null;
        
        _hasImage = _image != null;
    }

    // receive QR data as base64 string without prefix
    public void ReceiveQRData(string qrData)
    {
        byte[] bytes = Convert.FromBase64String(qrData);
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(bytes);
        // set as sprite to sprite renderer
        Sprite s = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        if (_hasImage)
        {
            _image.sprite = s;
        }
        
        if (_hasSpriteRenderer)
        {
            _spriteRenderer.sprite = s;
        }
    }

}
