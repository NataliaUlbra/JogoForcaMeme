using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public GameObject LoginScreen, GameScreen;
    public GameObject AdmScreen;
    public GameObject PalavrasScreen, DicasScreen, CategoriasScreen;
    public InputField UserName;
    #region Singleton
    //Singleton
    public static ButtonController Instance { get; protected set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    #endregion

    public void StarGame()
    {
        if (UserName.text == string.Empty)
        {
            return;
        }
        Instance.GameScreen.SetActive(true);
        Instance.LoginScreen.SetActive(false);
    }

    public void Adm_Screen()
    {
        var newStatus = !Instance.AdmScreen.activeSelf;
        Instance.AdmScreen.SetActive(newStatus);
    }

    public void CadastroNovasPalavras_Screen()
    {
        var newStatus = !Instance.PalavrasScreen.activeSelf;
        Instance.PalavrasScreen.SetActive(newStatus);
    }
    public void CadastroNovasDicas_Screen()
    {
        var newStatus = !Instance.DicasScreen.activeSelf;
        Instance.DicasScreen.SetActive(newStatus);
    }
    public void CadastroNovasCategorias_Screen()
    {
        var newStatus = !Instance.CategoriasScreen.activeSelf;
        Instance.CategoriasScreen.SetActive(newStatus);
    }
}
