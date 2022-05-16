using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    const int ACERTO_POR_LETRA = 5;
    const int ACERTO_PALAVRA = 100;
    [SerializeField] private GameObject PalavraGrid, EspacoGrid;
    [SerializeField] private GameObject LetraPrefab, EspacoPrefab;
    [SerializeField] private GameObject CaixaTextoLetrasEliminadas;
    [SerializeField] private GameObject CaixaTextoPontuacao;
    [SerializeField] private GameObject Stickerman;
    [SerializeField] private InputField CampoInputField;
    [SerializeField] private Button TryButton;
    private string Palavra;
    private string LetrasEliminadas;
    private int Vida;
    private int PontuacaoAtual;
    private int QtdAcertos;

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

    private void Start()//Inicializa a rodada
    {
        Instance.Vida = 6;
        Instance.LetrasEliminadas = "";
        Instance.PontuacaoAtual = 0; //Puxar do database
        Instance.QtdAcertos = 0;
        Instance.Palavra = ("pao").ToUpper();  //Puxar do database
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
        if (string.IsNullOrEmpty(CampoInputField.text))
            return;

        string palavra = Instance.Palavra;
        char letraDigitada = CampoInputField.text.ToUpper().ToCharArray()[0];//TODO: acentuação
        bool acertou = false;

        if (VerificaLetraRepetida(letraDigitada))//Verifica letra repetida e limpa
            return;

        for (int i = 0; i < palavra.Length; i++)
        {
            if (palavra[i] == letraDigitada)
            {
                PalavraGrid.transform.GetChild(i).gameObject.GetComponent<Text>().enabled = true;
                Instance.QtdAcertos++;
                acertou = true;
            }
        }
        Instance.LetrasEliminadas += letraDigitada; //Adiciona a letra na lista de tentativas
        Instance.CaixaTextoLetrasEliminadas.GetComponent<Text>().text = Instance.LetrasEliminadas;// TODO: REGEX
        LifeUpdate(acertou);
    }
    #region REGEX_ANDAMENTO
    //public string useRegex(String input)
    //{// Console.WriteLine("Returned string: " + Regex.Replace(uncPath, pattern, replacement, RegexOptions.IgnoreCase));

    //    var itensSeparados = texto.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
    //.Select(x => x.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
    //.ToList();
    //var testRegex = Regex.Replace(Instance.LetrasEliminadas, @"[^A-Z]", "-");
    //    Regex regexPattern = new Regex("([a-zA-Z](-[a-zA-Z])+)", RegexOptions.IgnoreCase);
    //    string pattern = @"([a-zA-Z](-[a-zA-Z])+)";
    //    string replacement = "$1";
    //    string input = "$16.32 12.19 £16.29 €18.29  €18,29";
    //    string result = Regex.Replace(input, pattern, replacement);
    //    Console.WriteLine(result);

    //    return result;
    //}
    #endregion
    public bool VerificaLetraRepetida(char letraDigitada) //Se a letra já foi digitada retorna true
    {
        Instance.CaixaTextoLetrasEliminadas.GetComponent<Text>().text = Instance.LetrasEliminadas;
        CampoInputField.text = string.Empty;//Limpa o campo onde o usuário digitada
        foreach (var letra in Instance.LetrasEliminadas)
        {
            if (letra == letraDigitada)
            {
                return true;
            }
        }
        return false;
    }
    public void LifeUpdate(bool acertou)
    {
        if (acertou)
        {
            Instance.PontuacaoAtual += ACERTO_POR_LETRA;
            Instance.CaixaTextoPontuacao.GetComponent<TextMeshProUGUI>().text = Instance.PontuacaoAtual.ToString();

            if (Instance.QtdAcertos == Instance.Palavra.Length)//Verifica vitoria
            {
                Instance.PontuacaoAtual += ACERTO_PALAVRA;
                Instance.CaixaTextoPontuacao.GetComponent<TextMeshProUGUI>().text = Instance.PontuacaoAtual.ToString();
                Debug.Log("VITORIA");
                //TODO: TELA REINICIA
            }
            return;
        }

        Instance.Vida--;
        Instance.Stickerman.transform.GetChild(Instance.Vida).gameObject.SetActive(false);
        if (Instance.Vida == 0)
        {
            Debug.Log("GAME OVER");
            Instance.Stickerman.GetComponent<Image>().color = Color.red;
            return;
            //TODO: TELA REINICIA
        }
    }
}