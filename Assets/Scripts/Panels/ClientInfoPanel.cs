using TMPro;
using UnityEngine;

public class ClientInfoPanel : MonoBehaviour, IPanel
{
    public TMP_Text _caseNumberText;
    public TMP_InputField _firstNameInput;
    public TMP_InputField _lastNameInput;
    public GameObject locationPanel;
    private void OnEnable()
    {
        _caseNumberText.text = "Case Number: " + UIManager.Instance.activeCase.caseID;
    }

    public void ProcessInfo()
    {
     //check if the first and last name are not empty

        if(string.IsNullOrEmpty(_firstNameInput.text) || string.IsNullOrEmpty(_lastNameInput.text))
        {
            //show error message. 
            Debug.Log("Either first or last name is empty");
            return;
        }
        else
        {
            UIManager.Instance.activeCase.caseName = _firstNameInput.text + " " + _lastNameInput.text;
            locationPanel.SetActive(true);
        }
    }
}
