using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocationPanel : MonoBehaviour,IPanel
{
    public TMP_Text caseNumberText;
    public RawImage _mapImage;
    public TMP_InputField _notesInput;

    public string apiKey;
    //comes from mobile device. 
    public float xCoord;
    public float yCoord;

    public int zoom;
    public int imageSize;
    public string url;

    private void OnEnable()
    {
        caseNumberText.text = "Case Number: " + UIManager.Instance.activeCase.caseID;
    }

    private IEnumerator Start()
    {
        if(Input.location.isEnabledByUser == true)
        {
            //start service
            Input.location.Start();
            
            int maxWait = 20;
            //PAUSE AND WAIT FOR INITIALIZATION TO STOP 
            while(Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1.0f);
                maxWait--;
            }

            if(maxWait < 1)
            {
                Debug.Log("Timed Out");
                yield break;
            }
            
            if(Input.location.status == LocationServiceStatus.Failed)
            {
                Debug.Log("FAILED to ffind location");
            }
            else
            {
                xCoord = Input.location.lastData.latitude;
                yCoord = Input.location.lastData.longitude;
            }
            //turn em off. 
            Input.location.Stop();
        }
        else
        {
            Debug.Log("Location Service are not on");
        }

        StartCoroutine(DownloadMap());
    }

    IEnumerator DownloadMap()
    {
        url = url + "center="
          + xCoord + ","
          + yCoord +
          "&zoom=" +
          zoom +
          "&size=" +
          imageSize +
          "x" +
          imageSize +
          "&key=" + apiKey;

        using (WWW map = new WWW(url))
        {
            yield return map;
            if (map.error != null)
            {
                Debug.LogError("Map Error: " + map.error);
            }

            _mapImage.texture = map.texture;
        }
        //apply map to raw image 
    }
    public void ProcessInfo()
    {
        if (string.IsNullOrEmpty(_notesInput.text) == false)
        {
            UIManager.Instance.activeCase.locationNotes = _notesInput.text;
        }
    }
}
