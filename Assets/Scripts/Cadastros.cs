using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cadastros : MonoBehaviour
{
    public InputField NewCategoria_InputField;

    public Dropdown NewPalavra_Dropdown;
    public InputField NewPalarva_InputField;

    public Dropdown NewDica_Dropdown;
    public InputField NewDica_InputField;
    public void CadastroNovaCategoria_Send()
    {
        if (NewCategoria_InputField.text == string.Empty)
        {
            return;
        }
        DbContext.Instance.InsertCategory(new CategoryViewModel(NewCategoria_InputField.text.ToUpper()));
        NewCategoria_InputField.text = string.Empty;
    }
    public void CadastroNovaPalavra_Send()
    {
        if (NewPalavra_Dropdown.value == 0 || NewPalarva_InputField.text == string.Empty)
        {
            return;
        }
        DbContext.Instance.InsertWords(new WordsViewModel(NewPalarva_InputField.text.ToUpper(), GetCategoriesId(NewPalavra_Dropdown.captionText.text)));
        NewPalarva_InputField.text = string.Empty;
    }

    public int GetCategoriesId(string categoryWord)
    {
        var categories = DbContext.Instance.GetCategories();
        foreach (var category in categories)
        {
            if (category.CategoryValue == categoryWord)
            {
                return category.Id;
            }
        }
        return 0;
    }

    public void CadastroNovaDica_Send()
    {
        if (NewDica_Dropdown.value == 0 || NewDica_InputField.text == string.Empty)
        {
            return;
        }
        DbContext.Instance.InsertTips(new TipsViewModel(NewDica_InputField.text, GetWordId(NewDica_Dropdown.captionText.text)));
        NewDica_InputField.text = string.Empty;
    }

    public int GetWordId(string wordGame)
    {
        var words = DbContext.Instance.GetWords();
        foreach (var word in words)
        {
            if (word.WordsValue == wordGame)
            {
                return word.WordsId;
            }
        }
        return 0;
    }
}
