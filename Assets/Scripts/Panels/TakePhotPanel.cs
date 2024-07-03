using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TakePhotPanel : MonoBehaviour, IPanel
{
    public TMP_Text caseNumberText;
    public RawImage _photoTaken;
    public TMP_InputField _photNotesInput;

    public GameObject overviewPanel;

    public string imgPath;

    private void OnEnable()
    {
        caseNumberText.text = "Case Number: " + UIManager.Instance.activeCase.caseID;
    }
    public void ProcessInfo()
    {
        byte[] imgData = null;

        if(string.IsNullOrEmpty(imgPath) == false)
        {
            //create a 2d tex 
            //apply from img path 
            //encode to png and store bytes  to photTaken of activeCase
            Texture2D img = NativeCamera.LoadImageAtPath(imgPath, 512, false);
            imgData = img.EncodeToPNG();
        }

        if(string.IsNullOrEmpty(_photNotesInput.text) == false)
        {
            UIManager.Instance.activeCase.photoNotes = _photNotesInput.text;
        }
        UIManager.Instance.activeCase.photoTaken = imgData;

        overviewPanel.SetActive(true);
    }

    public void TakePicture(int maxSize)
    {
        NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create a Texture2D from the captured image
                Texture2D texture = NativeCamera.LoadImageAtPath(path, maxSize, false);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                _photoTaken.gameObject.SetActive(true);
                _photoTaken.texture = texture;
                imgPath = path;

            }
        }, maxSize);

        Debug.Log("Permission result: " + permission);
    }
}
