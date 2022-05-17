using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CadastroPalavras : MonoBehaviour
{
    public Dropdown CategoriaList;
    private void OnEnable()
    {
        List<CategoryViewModel> categorias = DbContext.Instance.GetCategories();
        List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
        foreach (CategoryViewModel categoria in categorias)
        {
            optionDatas.Add(new Dropdown.OptionData(categoria.CategoryValue));
        }
        CategoriaList.ClearOptions();
        CategoriaList.AddOptions(optionDatas);
    }
}
