using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Build.DataBuilders;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

/// <summary>
/// Build scripts used for player builds and running with bundles in the editor.
/// </summary>
[CreateAssetMenu(fileName = "BuildScriptVideoPacker.asset", menuName = "Addressables/Content Builders/Video Build Script")]
public class BuildScriptVideoPacker : BuildScriptPackedMode
{
    [SerializeField]
    private string m_VideoExtension = ".mp4";
    
    /// <inheritdoc />
    public override string Name
    {
        get { return "Video Build Script"; }
    }
    
    private List<AddressableAssetEntry> m_VideoEntries;
    private List<AddressableAssetGroup> m_VideoEntriesGroup;

    private List<string> m_CreatedAssets;
    private List<AddressableAssetEntry> m_CreatedEntries;
    protected override TResult BuildDataImplementation<TResult>(AddressablesDataBuilderInput context)
    {
        m_CreatedAssets = new List<string>();
        m_CreatedEntries = new List<AddressableAssetEntry>();
        m_VideoEntries = new List<AddressableAssetEntry>();
        m_VideoEntriesGroup = new List<AddressableAssetGroup>();
        
        var result = base.BuildDataImplementation<TResult>(context);

        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
        DoCleanup(settings);
        return result;
    }
    
    protected override string ProcessGroup(AddressableAssetGroup assetGroup, AddressableAssetsBuildContext aaContext)
    {
        foreach (var assetEntry in assetGroup.entries)
        {
            if (assetEntry.AssetPath.Contains(m_VideoExtension))
            {
                var assetPath = CreateVideoURLAsset(assetEntry);
                m_CreatedAssets.Add(assetPath);
                m_VideoEntries.Add(assetEntry);
                m_VideoEntriesGroup.Add(assetGroup);
            }
        }

        foreach (var videoEntry in m_VideoEntries)
        {
            aaContext.Settings.RemoveAssetEntry(videoEntry.guid, false);
        }
        
        for (int i = 0; i < m_CreatedAssets.Count; i++)
        {
            var newVideoURLEntry = AddVideoEntry(m_CreatedAssets[i], m_VideoEntries[i], assetGroup, aaContext);
            m_CreatedEntries.Add(newVideoURLEntry);
        }
        
        return base.ProcessGroup(assetGroup, aaContext);
    }

    private string CreateVideoURLAsset(AddressableAssetEntry videoEntry)
    {
        var videoURL = CreateInstance<VideoURL>();

        var index = videoEntry.AssetPath.LastIndexOf(Path.DirectorySeparatorChar);
        var fileName = videoEntry.AssetPath.Substring(index + 1);
        videoURL.assetPath = fileName;
        
        var newFilePath = videoEntry.AssetPath.Replace("mp4", "asset");
        AssetDatabase.CreateAsset(videoURL, newFilePath);
        return newFilePath;
    }
    
    private AddressableAssetEntry AddVideoEntry(string assetPath, AddressableAssetEntry videoAddrEntry, AddressableAssetGroup group, AddressableAssetsBuildContext aaContext)
    {
        var guid = AssetDatabase.GUIDFromAssetPath(assetPath).ToString();
        var newEntry = aaContext.Settings.CreateOrMoveEntry(guid, group);
        newEntry.address = videoAddrEntry.address;
        return newEntry;
    }

    void DoCleanup(AddressableAssetSettings settings)
    {
        foreach (var createdEntry in m_CreatedEntries)
        {
            settings.RemoveAssetEntry(createdEntry.guid);
            AssetDatabase.DeleteAsset(createdEntry.AssetPath);
        }
        
        for(int i = 0; i < m_VideoEntries.Count; i++)
        {
            settings.CreateOrMoveEntry(m_VideoEntries[i].guid, m_VideoEntriesGroup[i]);
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}

