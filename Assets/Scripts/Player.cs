using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("PlayerUI")]
    public GameObject selectionPanel;
    public Text infoText;
    public Image currentPickImage;
    public List<Image> stars;

    [Space]
    public int roundsWon;
    public bool isSelecting = false;
    public int pickNum;
    public Sprite currentPick;

    private Shadow panelOutline;

    void Start()
    {
        roundsWon = 0;
    }


    public void DisableSelection()
    {
        selectionPanel.SetActive(false);
        infoText.gameObject.SetActive(true);
        currentPickImage.gameObject.SetActive(false);

        panelOutline = selectionPanel.GetComponentInParent<Shadow>();
        panelOutline.enabled = false;
    }

    public void EnableSelection()
    {
        selectionPanel.SetActive(true);
        infoText.gameObject.SetActive(false);
        currentPickImage.gameObject.SetActive(false);

        panelOutline = selectionPanel.GetComponentInParent<Shadow>();
        panelOutline.enabled = true;
    }

    public void ShowPlayerPickOnEnd()
    {
        selectionPanel.SetActive(false);
        infoText.gameObject.SetActive(false);

        currentPickImage.gameObject.SetActive(true);
        currentPickImage.sprite = currentPick;
    }

    // This function is attached to buttons and returns selected character.
    public void GetPlayerPick(int value)
    {
        pickNum = value;

        isSelecting = false;

        Transform buttonsHolder;
        buttonsHolder = selectionPanel.transform.GetChild(0);

        Transform selectedButton;
        selectedButton = buttonsHolder.transform.GetChild(pickNum);
        currentPick = selectedButton.GetChild(0).GetComponentInChildren<Image>().sprite;
    }
}
