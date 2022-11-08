using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "VideoURL", menuName = "ScriptableObjects/Video URL", order = 1)]
public class VideoURL : ScriptableObject
{
    private const string k_ServerURL = "www.mockup-server.com";
    private const string k_VideosPath = "videos";
    
    [SerializeField]
    private string m_AssetName;

    [SerializeField]
    private string m_VideoURL;
    
    public string assetPath
    {
        set
        {
            m_AssetName = value;
            m_VideoURL = Path.Combine(k_ServerURL, k_VideosPath, m_AssetName);
        }
    }
    
    public string URL => m_VideoURL;
}
