using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class UIManager_05 : MonoBehaviour
{
    public List<GameObject> Backgrounds;
    public GameObject Light;

    public GameObject Script_Image;
    [SerializeField]
    public ScriptS Scrs = new ScriptS();

    public GameObject BlackBG;
    public GameObject ResultPU;
    void Start()
    {
        //시나리오에 맞는 배경 켜기
        foreach (var Obj in Backgrounds)
        {
            Obj.SetActive(false);
        }
        Backgrounds[GameManager.Instance.NowScenario.No - 1].SetActive(true);

        Light.GetComponent<SpriteRenderer>().DOFade(0, 5).SetLoops(-1, LoopType.Yoyo);

        StartCoroutine(StartDelay());
    }

    void Update()
    {

    }

    public IEnumerator StartDelay()
    {
        BlackBG.GetComponent<Image>().DOFade(0, 2);
        yield return new WaitForSeconds(2f);
        BlackBG.SetActive(false);
        Script_Image.SetActive(true);

        TextMeshProUGUI tmp = Script_Image.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        foreach (var Scr in Scrs.scripts[GameManager.Instance.NowScenario.No - 1].script)
        {
            tmp.text  = Scr;
            yield return new WaitForSeconds(4f);
            //while (!Input.GetMouseButtonDown(0))
            //{ }
        }
        BlackBG.SetActive(true);
        BlackBG.GetComponent<Image>().DOFade(0.5f, 2);
        yield return new WaitForSeconds(2f);
        ResultPU.SetActive(true);
    }

    public void To_Ingame()
    {
        GameManager.Instance.ScenarioIllust[GameManager.Instance.NowScenario.No - 1] = true;
        GameManager.Instance.Reset();
        LoadManager.LoaderCallback();
        LoadManager.Load(LoadManager.Scene.Ingame);
    }
}
