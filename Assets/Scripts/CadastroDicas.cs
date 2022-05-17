using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CadastroDicas : MonoBehaviour
{
    public Dropdown PalavrasList;
    private void OnEnable()
    {
        List<WordsViewModel> words = DbContext.Instance.GetWords();
        List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
        foreach (WordsViewModel word in words)
        {
            optionDatas.Add(new Dropdown.OptionData(word.WordsValue));
        }
        PalavrasList.ClearOptions();
        PalavrasList.AddOptions(optionDatas);
    }
}
