using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
  
  public AudioMixer audioMixer;

  public Dropdown resolutionDropdown;
  
  Resolution[] resolutions;

  void Start(){

    resolutions = Screen.resolutions ;

    resolutionDropdown.ClearOptions();

    List<string> options = new List<string>();

    int currentResolutionIndex = 0;

    for(int i=0; i<resolutions.Length; i++){

        string option = resolutions[i].width+" x "+resolutions[i].height; 
        options.Add(option);

        if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height ){

            currentResolutionIndex = i;

        }

    }

    resolutionDropdown.AddOptions(options);
    resolutionDropdown.value = currentResolutionIndex;
    resolutionDropdown.RefreshShownValue();
  

  }

  public void SetResolution(int resolutionIndex){

    Resolution resolution = resolutions[resolutionIndex];
    Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

  }
  public void ExitButton(){

      Application.Quit();
      Debug.Log("Gioco Chiuso");

  }

  public void StartGame(){

      SceneManager.LoadScene(1);
      Debug.Log("Gioco Avviato");

  }
  public void StartOption(){

      
      Debug.Log("Opzioni Avviato");

  }
    public void StartSearchLastGame(){

     
      Debug.Log("Gioco Avviato dall'ultimo salvataggio");

  }

  public void  SetVolume(float volume){
   
   audioMixer.SetFloat("Volume", volume);
   Debug.Log(volume);

  }

  public void SetFullScreen(bool isFullScreeen){

    Screen.fullScreen = isFullScreeen;

  }




}
