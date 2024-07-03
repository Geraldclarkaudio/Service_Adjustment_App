using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    { get 
        { 
            if (_instance == null)
            {
                Debug.LogError("UIManager is NULL");
            }

            return _instance; 
        } 
    }

    public Case activeCase;
    public ClientInfoPanel clientInfoPanel;
    public GameObject borderPanel;

    private void Awake()
    {
        _instance = this;
    }

    public void CreateNewCase()
    {
        ///set active case to a new case. 
        activeCase = new Case();
        //generate case ID between 0 - 999
        int randomCaseID = Random.Range(0, 1000);
        activeCase.caseID = randomCaseID.ToString();

        clientInfoPanel.gameObject.SetActive(true);
        borderPanel.SetActive(true);
    }

    public void SubmitButton()
    {
        //create a new case to save
        //populate case data 
        //open data stream to turn object into a file. 
        //begin aws process. 
        
        //1 ...
        Case awsCase = new Case();
        //2...
        awsCase.caseID = activeCase.caseID;
        awsCase.date = activeCase.date;
        awsCase.locationNotes = activeCase.locationNotes;
        awsCase.photoNotes = activeCase.photoNotes;
        awsCase.photoTaken = activeCase.photoTaken;
        //3...data stream 
        BinaryFormatter bf = new BinaryFormatter();
        //creates dat file named after case ID. 

        string filePath = Application.persistentDataPath + "/case" + awsCase.caseID + ".dat";
        FileStream file = File.Create(filePath);

        //write to this file. 
        bf.Serialize(file, awsCase);
        file.Close();

        //send to aws 
        AWSManager.Instance.UploadToS3(filePath, awsCase.caseID);
    }
}
