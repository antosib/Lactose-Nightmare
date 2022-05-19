using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{

    public static LevelLoader instance;

    [SerializeField] private GameObject loaderCanvas;
    [SerializeField] private Slider progressBar;

    private float target;

    private void Awake()
    {
        
        if(instance == null){

            instance = this;

            DontDestroyOnLoad(gameObject);

        }else{

            Destroy(gameObject);

        }

    }

    public async void LoadScene(string sceneName){

        progressBar.value = 0;
        target = 0;
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false ;

        loaderCanvas.SetActive(true);

        do{

            await Task.Delay(100);
            target = scene.progress;

        }while(scene.progress < 0.9f);

        await Task.Delay(1000);

        scene.allowSceneActivation = true ;

        loaderCanvas.SetActive(false);

    }

    void Update()
    {
        
      progressBar.value = Mathf.MoveTowards(progressBar.value,target,3 * Time.deltaTime);

    }



}
