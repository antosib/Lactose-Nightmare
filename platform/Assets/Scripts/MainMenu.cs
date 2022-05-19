using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
   
  

  public GameObject OptionMenu, ResoluctionMenu, AudioMenu, ExitMenu;

  public GameObject fistrInOptionMenu, fistrInResoluctionMenu, fistrInAudioMenu, OptionInMainMenu, ResoluctionInOptionMenu, AudioInOptionMenu, ExitInMainMenu, fistrInExitMenu;

  public Toggle fullScreenTog;

  public AudioMixer audioMixer;

  public Slider musicSlider, effectSlider, generalSlider;

  public Dropdown resolutionDropdown;
  
  Resolution[] resolutions;

  void Start(){

    fullScreenTog.isOn = Screen.fullScreen;

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

  private void Awake()
  {
    
    musicSlider.onValueChanged.AddListener(SetVolumeMusic);
    effectSlider.onValueChanged.AddListener(SetVolumeEffect);
    generalSlider.onValueChanged.AddListener(SetVolumeGeneral);

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

  public void  SetVolumeMusic(float volume){
   
   audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume)*20);
   Debug.Log(volume);

  }

   public void  SetVolumeEffect(float volume){
   
   audioMixer.SetFloat("EffectVolume", Mathf.Log10(volume)*20);
   Debug.Log(volume);

  }

   public void  SetVolumeGeneral(float volume){
   
   audioMixer.SetFloat("GeneralVolume", Mathf.Log10(volume)*20);
   Debug.Log(volume);

  }

  public void SetFullScreen(bool isFullScreeen){

    Screen.fullScreen = isFullScreeen;

  }

  public void OpenOptionMenu(){

      OptionMenu.SetActive(true);

      EventSystem.current.SetSelectedGameObject(null);
      EventSystem.current.SetSelectedGameObject(fistrInOptionMenu);
      
  }

  public void CloseOptionMenu(){

      OptionMenu.SetActive(false);

      EventSystem.current.SetSelectedGameObject(null);
      EventSystem.current.SetSelectedGameObject(OptionInMainMenu);
      
  }

  public void OpenResoluctionMenu(){

      ResoluctionMenu.SetActive(true);

      EventSystem.current.SetSelectedGameObject(null);
      EventSystem.current.SetSelectedGameObject(fistrInResoluctionMenu);
      
  }

    public void CloseResoluctionMenu(){

      ResoluctionMenu.SetActive(false);

      EventSystem.current.SetSelectedGameObject(null);
      EventSystem.current.SetSelectedGameObject(ResoluctionInOptionMenu);
      
  }
  
  public void OpenAudioMenu(){

      AudioMenu.SetActive(true);

      EventSystem.current.SetSelectedGameObject(null);
      EventSystem.current.SetSelectedGameObject(fistrInAudioMenu);
      
  }

  public void CloseAudioMenu(){

      AudioMenu.SetActive(false);

      EventSystem.current.SetSelectedGameObject(null);
      EventSystem.current.SetSelectedGameObject(AudioInOptionMenu);
      
  }

  public void OpenExitMenu(){

      ExitMenu.SetActive(true);

      EventSystem.current.SetSelectedGameObject(null);
      EventSystem.current.SetSelectedGameObject(fistrInExitMenu);

     
  }
  public void CloseExitMenu(){

      ExitMenu.SetActive(false);

      EventSystem.current.SetSelectedGameObject(null);
      EventSystem.current.SetSelectedGameObject(ExitInMainMenu);
      
  }





}

