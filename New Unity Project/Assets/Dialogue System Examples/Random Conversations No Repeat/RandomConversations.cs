using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

public class RandomConversations : MonoBehaviour
{

    public int numConversations;

    private void Start()
    {
        // Initialize the conversationNumbers list:
        var orderedList = new List<int>();
        for (int i = 0; i < numConversations; i++)
        {
            orderedList.Add(i + 1);
        }

        // Put it into Lua as an array named "convNum[]":
        var luaCode = "convNum = {";
        foreach (var convNum in orderedList.OrderBy(x => Random.value))
        {
            luaCode += convNum + ",";
        }
        luaCode += "}";
        Lua.Run(luaCode);

        // And set the index position to the first element of the array:
        Lua.Run("convIndex = 1", true);
    }

}
