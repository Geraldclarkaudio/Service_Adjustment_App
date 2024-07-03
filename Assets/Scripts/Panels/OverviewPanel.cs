using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OverviewPanel : MonoBehaviour,IPanel
{
    public TMP_Text _caseNumberText;
    public TMP_Text _nameText;
    public TMP_Text _dateText;
    public TMP_Text _LocationNotesText;
    public TMP_Text _photoNotesText;

    public RawImage _photoTaken;

    private void OnEnable()
    {
        _caseNumberText.text = "Case Number: " + UIManager.Instance.activeCase.caseID;
        _nameText.text = UIManager.Instance.activeCase.caseName;
        _dateText.text = DateTime.Today.TimeOfDay.ToString();
        _LocationNotesText.text = "LOCATION NOTES: \n" + UIManager.Instance.activeCase.locationNotes;
        _photoNotesText.text = "PHOTO NOTES: \n" + UIManager.Instance.activeCase.photoNotes;

        //need to set the photo
        if(UIManager.Instance.activeCase.photoTaken != null)
        {
            //rebuild photo and display it... 
            //convert bytes to PNG and then convert that to texture. 
            Texture2D reconstructedImage = new Texture2D(1, 1);
            reconstructedImage.LoadImage(UIManager.Instance.activeCase.photoTaken);
            _photoTaken.texture = (Texture)reconstructedImage;
        }
    }
    public void ProcessInfo()
    {
    }
}
