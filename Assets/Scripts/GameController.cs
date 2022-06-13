using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    const int ACERTO_POR_LETRA = 5;
    const int ACERTO_PALAVRA = 100;
    [SerializeField] private GameObject PalavraGrid, EspacoGrid;
    [SerializeField] private GameObject LetraPrefab, EspacoPrefab;
    [SerializeField] private GameObject CaixaTextoLetrasEliminadas;
    [SerializeField] private GameObject CaixaTextoPontuacao, CaixaTextoPontuacaoTotal;
    [SerializeField] private GameObject Stickerman;
    [SerializeField] private GameObject CategoriaName;
    [SerializeField] private InputField CampoInputField;
    [SerializeField] private Button TryButton;
    private string NomeCategoria;
    private string UserName;
    private string Palavra;
    private string LetrasEliminadas;
    private int Vida;
    private int PontuacaoAtual = 0;
    private int PontuacaoTotal = 0;
    private int QtdAcertos;
    private List<Assets.Scripts.Model.WordsViewModel> TodasPalavras = new List<Assets.Scripts.Model.WordsViewModel>();
    #region Vitoria_Screen
    [SerializeField] private GameObject VitoriaScreen;
    [SerializeField] private Button EncerrarJogoButton;
    [SerializeField] private Button ContinuarJogoButton;
    #endregion
    #region FimDejogo_Screen
    [SerializeField] private GameObject FimDejogoScreen;
    [SerializeField] private GameObject NameScorePrefab;
    [SerializeField] private GameObject ScoreGrid;
    [SerializeField] private Button FimDejogoButton;
    #endregion

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

    public void Start()//Inicializa o jogo
    {
        if (ButtonController.Instance.Categoria_Dropdown.value != 0)
        {
            Instance.NomeCategoria = ButtonController.Instance.Categoria_Dropdown.captionText.text;
            Assets.Scripts.Model.CategoryViewModel categoriaSelecionada = DbContext.Instance.GetCategories().Where(x => x.CategoryValue == Instance.NomeCategoria).FirstOrDefault();
            Instance.TodasPalavras = GetAllWords().Where(x => x.CategoryId == categoriaSelecionada.Id).ToList();
        }
        else
        {
            Instance.TodasPalavras = GetAllWords();
        }

        Instance.UserName = ButtonController.Instance.UserName.text;
        Instance.Vida = 6;
        Instance.LetrasEliminadas = "";
        Instance.QtdAcertos = 0;
        Instance.Palavra = RandomWord();
        TryButton.onClick.AddListener(TryLetra);
        GenerateGrid();
    }
    public void Restart()
    {
        ClearScreen();
        Instance.Vida = 6;
        Instance.QtdAcertos = 0;
        Instance.PontuacaoAtual = 0;
        Instance.Palavra = RandomWord();
        TryButton.onClick.AddListener(TryLetra);
        GenerateGrid();
    }
    public void ClearScreen()
    {
        Instance.TryButton.onClick.RemoveAllListeners();
        Instance.EncerrarJogoButton.onClick.RemoveAllListeners();
        Instance.ContinuarJogoButton.onClick.RemoveAllListeners();
        Instance.CaixaTextoPontuacao.GetComponent<TextMeshProUGUI>().text = "";
        Instance.LetrasEliminadas = "";

        for (int i = 0; i < PalavraGrid.transform.childCount; i++)
        {
            Destroy(PalavraGrid.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < EspacoGrid.transform.childCount; i++)
        {
            Destroy(EspacoGrid.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < Instance.Stickerman.transform.childCount; i++)
        {
            Instance.Stickerman.transform.GetChild(i).gameObject.SetActive(true);
        }

        Instance.VitoriaScreen.SetActive(false);
    }
    private List<Assets.Scripts.Model.WordsViewModel> GetAllWords()
    {
        var words = DbContext.Instance.GetWords();
        words.RemoveAt(0);
        return words;
    }
    public string RandomWord()
    {
        var randomValue = Random.Range(0, Instance.TodasPalavras.Count);
        var randomWord = Instance.TodasPalavras[randomValue];
        Instance.TodasPalavras.Remove(TodasPalavras[randomValue]);
        Assets.Scripts.Model.CategoryViewModel categoriaSelecionada = 
            DbContext.Instance.GetCategories().Where(x => x.Id == randomWord.CategoryId).FirstOrDefault();
        
        Instance.CategoriaName.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = categoriaSelecionada.CategoryValue;
        
        return randomWord.WordsValue;
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
                Instance.PontuacaoAtual = ACERTO_PALAVRA;
                Instance.CaixaTextoPontuacao.GetComponent<TextMeshProUGUI>().text = Instance.PontuacaoAtual.ToString();
                VITORIA();
            }
            return;
        }

        Instance.Vida--;
        Instance.Stickerman.transform.GetChild(Instance.Vida).gameObject.SetActive(false);
        if (Instance.Vida == 0)
        {
            Instance.PontuacaoTotal += Instance.PontuacaoAtual;
            Instance.CaixaTextoPontuacaoTotal.GetComponent<TextMeshProUGUI>().text = Instance.PontuacaoTotal.ToString();
            FimDejogo();
            return;
        }
    }
    public void VITORIA()
    {
        Instance.PontuacaoTotal += Instance.PontuacaoAtual;
        Instance.CaixaTextoPontuacaoTotal.GetComponent<TextMeshProUGUI>().text = Instance.PontuacaoTotal.ToString();
        Instance.LetrasEliminadas = "";
        if (Instance.TodasPalavras.Count > 0)
        {
            Instance.VitoriaScreen.SetActive(true);
            Instance.ContinuarJogoButton.onClick.AddListener(Restart);
            Instance.EncerrarJogoButton.onClick.AddListener(FimDejogo);
            return;
        }
        FimDejogo();
    }
    public void FimDejogo()
    {
        //mostrar pontuacao
        Instance.FimDejogoScreen.SetActive(true);
        Instance.FimDejogoButton.onClick.AddListener(RetornaMenuPrincipal);
        DbContext.Instance.InsertLeaderboard(new Assets.Scripts.Model.LeaderboardViewModel(Instance.UserName, Instance.PontuacaoTotal));
        var leaderboard = DbContext.Instance.GetLeaderboard().OrderByDescending(x => x.Score).ToList();

        foreach (var leader in leaderboard)
        {
            if (Instance.ScoreGrid.transform.childCount >= 10)
            {
                break;
            }

            var _nameLeader = Instantiate(NameScorePrefab);
            _nameLeader.transform.GetChild(0).GetComponent<Text>().text = leader.UserName;
            _nameLeader.transform.GetChild(1).GetComponent<Text>().text = leader.Score.ToString();
            _nameLeader.transform.SetParent(Instance.ScoreGrid.transform);
        }
    }
    public void RetornaMenuPrincipal()
    {
        Instance.FimDejogoScreen.SetActive(false);
        Instance.VitoriaScreen.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}