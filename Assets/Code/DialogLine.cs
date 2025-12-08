using UnityEngine;

[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialogue/Line")]
public class DialogLine : ScriptableObject
{
    [Header("Dialog Content")]
    public string characterName; 
    [TextArea] public string dialogText;
    public DialogChoice[] choices;
    public DialogLine defaultNextLine;
    
    [Header("Inventory Actions")]
    public ItemData itemToGive;
    public ItemData itemToTake; 
}