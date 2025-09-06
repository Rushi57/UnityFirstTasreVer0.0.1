using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "RecipieSO", menuName = "Cookbook/Recipie")]
public class RecipieSO : ScriptableObject
{
    public string recipeName;
    [TextArea] public string description;
    public List<string> steps;

    
}
