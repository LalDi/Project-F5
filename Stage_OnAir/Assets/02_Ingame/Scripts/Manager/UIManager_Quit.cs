using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_Quit : MonoBehaviour
{
    [SerializeField] private GameObject Popup_Quit;

    private bool isOnPopup;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Popup_Quit.SetActive(!isOnPopup);
            isOnPopup = !isOnPopup;
        }
    }

    public void QuitApplication()
    {
        //GameManager.Instance.SaveData();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }

    public void QuitPopup()
    {
        Popup_Quit.SetActive(false);
        isOnPopup = false;
    }
}
