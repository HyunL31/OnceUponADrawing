using UnityEngine;

[CreateAssetMenu(fileName = "StoryData", menuName = "Scriptable Objects/StoryData")]
public class StoryData : ScriptableObject
{
    public int id;
    public string storyCode;
    public string storyName;
    public string sceneName;
    public Sprite titleImage;
    public Sprite characterImage;
    public bool isImplemented;
}
