using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System.IO;
using System;
using Amazon.S3.Util;
using System.Collections.Generic;
using Amazon.CognitoIdentity;
using Amazon;
using UnityEngine.SceneManagement;

public class AWSManager : MonoBehaviour
{
    private static AWSManager _instance;
    public static AWSManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("AWS Manager is Null");
            }

            return _instance;
        }
    }

    public string s3Region = RegionEndpoint.USEast1.SystemName;
    
    private RegionEndpoint _s3Region
    {
        get
        {
            return RegionEndpoint.GetBySystemName(s3Region);
        }
    }
    private AmazonS3Client _s3Client;
    public AmazonS3Client S3Client
    {
        get
        {
            if (_s3Client == null)
            {
                _s3Client = new AmazonS3Client(
                    new CognitoAWSCredentials("us-east-1:f98771d9-e73a-4422-81c6-ddcfb9716f8d",
                    RegionEndpoint.USEast1),
                    _s3Region
                );
            }
            return _s3Client; // Return the client even if it's already initialized
        }
    }

    public GameObject confirm1;
    public GameObject confirm2;

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        UnityInitializer.AttachToGameObject(this.gameObject);
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
    }

    public void UploadToS3(string path, string caseID)
    {
        Debug.Log("I AM BEING CALLED!");
        confirm1.SetActive(true);
        //open file stream 
        FileStream stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
        //post object to s3 server 
        PostObjectRequest request = new PostObjectRequest()
        {
            Bucket = "serviceadjustmentappcasefiles1",
            Key = "case" + caseID,
            InputStream = stream,
            CannedACL = S3CannedACL.Private,
            Region = _s3Region
        };

        confirm2.SetActive(true);

        S3Client.PostObjectAsync(request, (responseObject) =>
        {
            Debug.Log("Made it to PostObject");
           
            //if no erros
            if (responseObject.Exception == null)
            {
                Debug.Log("Posted to Bucket");
                //reload the scene
                SceneManager.LoadScene(0);
            }
            else
            {
                Debug.Log("Failed Posting to Bucket " + responseObject.Exception);
            }
        });
    }

}
