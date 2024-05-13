using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class toGame : MonoBehaviour
{
    public void ChangeScenes(int numberScenes)
    {
        SceneManager.LoadScene(numberScenes);
    }

    
    private GameObject lt_post;
    private GameObject lt;
    private GameObject tmt_post;
    private GameObject tmt;
    private GameObject bell_post;
    private GameObject bell;
    private GameObject cdt_post;
    private GameObject cdt;

void Start()
{
    lt_post = GameObject.Find("Canvas/NotMainMenu/ButtonLT (Post)");
    lt = GameObject.Find("Canvas/NotMainMenu/ButtonLT");
    tmt_post = GameObject.Find("Canvas/NotMainMenu/ButtonTMT (Post)");
    tmt = GameObject.Find("Canvas/NotMainMenu/ButtonTMT");
    bell_post = GameObject.Find("Canvas/NotMainMenu/ButtonBT (Post)");
    bell = GameObject.Find("Canvas/NotMainMenu/ButtonBT");
    cdt_post = GameObject.Find("Canvas/NotMainMenu/ButtonCDT (Post)");
    cdt = GameObject.Find("Canvas/NotMainMenu/ButtonCDT");

    Debug.Log(lt_post);
}

    void Update(){

        Debug.Log(DataTransfer.state_line);

    if (DataTransfer.state_line && lt_post != null)
    {
        lt_post.SetActive(true);
        lt.SetActive(false);  
    }
    if (DataTransfer.state_tmt_a && DataTransfer.state_tmt_b && lt_post != null)
    {
        tmt_post.SetActive(true);
        tmt.SetActive(false);  
    }
    if (DataTransfer.state_bell && bell_post != null)
    {
        bell_post.SetActive(true);
        bell.SetActive(false);  
    }
    if (DataTransfer.state_cdt && cdt_post != null)
    {
        cdt_post.SetActive(true);
        cdt.SetActive(false);  
    }



    } 
}
