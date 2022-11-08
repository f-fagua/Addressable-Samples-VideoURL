using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Loader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Addressables.LoadAssetAsync<VideoURL>("Assets/Video/video.mp4").Completed += handle =>
        {
            Debug.Log(handle.Result.URL);
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
