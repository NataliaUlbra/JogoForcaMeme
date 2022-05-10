using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]private GameObject PalavraGrid, EspacoGrid;
    [SerializeField]private GameObject LetraPrefab, EspacoPrefab;
    [SerializeField]private InputField CampoInputField;
    [SerializeField]private Button TryButton;
    [SerializeField]string Palavra;
    #region Singleton
    //Singleton
    public static GameController Instance { get; protected set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    #endregion

    private void Start()
    {
        Instance.Palavra = ("sucesso").ToUpper();  //Puxar do database
        GenerateGrid();
        TryButton.onClick.AddListener(TryLetra);
    }

    public void GenerateGrid()
    {
        string palavra = Instance.Palavra;
        foreach (var letra in palavra)
        {
            var _letra = Instantiate(LetraPrefab); //Cria um novo objeto clonado do prefab
            _letra.GetComponent<Text>().text = letra.ToString(); //Altera o texto do objeto para a nova letra
            _letra.name = letra.ToString(); //Renomeia o objeto para a nova letra
            _letra.transform.SetParent(PalavraGrid.transform); //Coloca o objeto como filho do grid da palavra

            Instantiate(EspacoPrefab).transform.SetParent(EspacoGrid.transform); //Cria o espaço para aquela palavra
        }
    }

    public void TryLetra()
    {
        string palavra = Instance.Palavra;
        char letraDigitada = CampoInputField.text.ToUpper().ToCharArray()[0];//TODO: acentuação

        for (int i = 0; i < palavra.Length; i++)
        {
            if (palavra[i] == letraDigitada)
            {
                PalavraGrid.transform.GetChild(i).gameObject.GetComponent<Text>().enabled = true;
            }
        }
        CampoInputField.text = string.Empty;
    }
}
