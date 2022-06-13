using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public GameObject LoginScreen, GameScreen;
    public GameObject AdmScreen;
    public GameObject PalavrasScreen, DicasScreen, CategoriasScreen, ExitScreen, SelecionarCategoriaScreen;
    public InputField UserName;
    public Dropdown Categoria_Dropdown;
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
        Instance.SelecionarCategoriaScreen.SetActive(false);
    }
    public void StarGamePorCategoria()
    {
        if (UserName.text == string.Empty || ButtonController.Instance.Categoria_Dropdown.value == 0)
        {
            return;
        }
        Instance.GameScreen.SetActive(true);
        Instance.LoginScreen.SetActive(false);
        Instance.SelecionarCategoriaScreen.SetActive(false);
    }

    public void Adm_Screen()
    {
        var newStatus = !Instance.AdmScreen.activeSelf;
        Instance.AdmScreen.SetActive(newStatus);
    }
    public void SelecionarCategoria_Screen()
    {
        if (UserName.text == string.Empty)
        {
            return;
        }
        var newStatus = !Instance.SelecionarCategoriaScreen.activeSelf;
        Instance.SelecionarCategoriaScreen.SetActive(newStatus);
    }

    public void Exit_Screen()
    {
        var newStatus = !Instance.ExitScreen.activeSelf;
        Instance.ExitScreen.SetActive(newStatus);
    }
    public void SAIR_JOGO()
    {
        Application.Quit();
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
