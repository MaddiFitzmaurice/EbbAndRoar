using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField] GameObject _toBeContinuedUI;
    [SerializeField] Image _blackFade;

    void Start()
    {
        _toBeContinuedUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _toBeContinuedUI.SetActive(true);
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        Vector4 colour = Vector4.zero;
        float time = 0;
        
        while (time < 3f)
        {
            _blackFade.color = Vector4.Lerp(colour, Color.black, time / 3f);
            time += Time.deltaTime;
            yield return null;
        }

        _blackFade.color = Color.black; 
        SceneManager.LoadScene(0);     
    }
}
