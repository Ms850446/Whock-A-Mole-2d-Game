using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class startGame : MonoBehaviour
{
    // Start is called before the first frame update
    public void onClickButton(){
        SceneManager.LoadScene(1);
    }

}
