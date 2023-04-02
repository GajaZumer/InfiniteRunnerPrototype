using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //list of UI elements that represent obtained oxygen 
    public List<GameObject> UIOxygen;

    public Text txtInstruction;
    public Text txtHeartRate;
    public Text txtClotsEliminated;
    public GameObject EndgameCanvas;

    List<string> instructionsList = new List<string>() { "", "collect OXYGEN to gain power", "press SPACE to dash" };

    private float heartRate;
    private int maxNumOxygen;
    private bool showDashInstruction = true;

    void Start()
    {
        maxNumOxygen = UIOxygen.Count;

        //show txt instruction about collecting oxygen
        txtInstruction.text = instructionsList[1];

        //change heart rate txt
        txtHeartRate.text = Heart.HeartRate.ToString() + " BPM";

        if (PlayerController.current != null)
        {
            PlayerController.current.onOxygenCollide += onOxygenCollide;
            PlayerController.current.onClotCollide += onClotCollide;
            PlayerController.current.onDash += onDash;
        }
    }

    void OnDisable()
    {
        PlayerController.current.onOxygenCollide -= onOxygenCollide;
        PlayerController.current.onClotCollide -= onClotCollide;
        PlayerController.current.onDash -= onDash;
    }

    private void onOxygenCollide()
    {
        if (PlayerController.NumOxygen <= maxNumOxygen)
        {
            //show dash charge
            UIOxygen[PlayerController.NumOxygen - 1].SetActive(true);

            if (showDashInstruction)
            {
                //show txt instruction how to dash
                txtInstruction.text = instructionsList[2];
                showDashInstruction = false;
            }
        }
    }

    private void onClotCollide(int state)
    {
        //collision with clot and NO dashing
        if (state == 0)
        {
            StartCoroutine(WaitForEndgame());
        }
        //collision with clot while dashing
        else if (state == 1)
        {
            //update score text
            txtClotsEliminated.text = PlayerController.NumClots.ToString() + " x";
        }
    }

    IEnumerator WaitForEndgame()
    {
        yield return new WaitForSeconds(0.3f);
        //activate endgame canvas
        EndgameCanvas.SetActive(true);
    }

    private void onDash()
    {
        if ((PlayerController.NumOxygen > 0) && (PlayerController.NumOxygen <= maxNumOxygen))
        {
            //hide one dash charge
            UIOxygen[PlayerController.NumOxygen - 1].SetActive(false);

            if (!showDashInstruction)
            {
                //hide txt instruction how to dash
                txtInstruction.text = instructionsList[0];
            }
        }
    }

    //pressed Replay btn
    public void Replay()
    {
        SceneManager.LoadScene("Game");
    }
}
