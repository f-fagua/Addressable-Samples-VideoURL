using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build.Pipeline.Interfaces;
using UnityEngine;

namespace UnityEditor.AddressableAssets.Build.DataBuilders
{
    using Debug = UnityEngine.Debug;

    /// <summary>
    /// Build scripts used for player builds and running with bundles in the editor.
    /// </summary>
    [CreateAssetMenu(fileName = "BuildScriptVideoPacker.asset", menuName = "Addressables/Content Builders/Video Build Script")]
    public class BuildScriptVideoPacker : BuildScriptPackedMode
    {
        /// <inheritdoc />
        public override string Name
        {
            get { return "Video Build Script"; }
        }
        
        /// <inheritdoc />
        protected override TResult BuildDataImplementation<TResult>(AddressablesDataBuilderInput builderInput)
        {
            TResult result = default(TResult);
            m_IncludedGroupsInBuild?.Clear();

            InitializeBuildContext(builderInput, out AddressableAssetsBuildContext aaContext);

            using (m_Log.ScopedStep(LogLevel.Info, "ProcessAllGroups"))
            {
                var errorString = ProcessAllGroups(aaContext);
                if (!string.IsNullOrEmpty(errorString))
                    result = CreateErrorResult<TResult>(errorString, builderInput, aaContext);
            }

            if (result == null)
            {
                result = DoBuild<TResult>(builderInput, aaContext);
            }

            if (result != null)
            {
                var span = DateTime.Now - aaContext.buildStartTime;
                result.Duration = span.TotalSeconds;
            }

            if (result != null && string.IsNullOrEmpty(result.Error))
            {
                foreach (var group in m_IncludedGroupsInBuild)
                    ContentUpdateScript.ClearContentUpdateNotifications(group);
            }

            if (result != null && string.IsNullOrEmpty(result.Error))
            {
                foreach (var group in m_IncludedGroupsInBuild)
                    ContentUpdateScript.ClearContentUpdateNotifications(group);
            }

            return result;
        }

        protected override string ProcessAllGroups(AddressableAssetsBuildContext aaContext)
        {
            return base.ProcessAllGroups(aaContext);
        }

        protected override string ProcessGroup(AddressableAssetGroup assetGroup, AddressableAssetsBuildContext aaContext)
        {
            return base.ProcessGroup(assetGroup, aaContext);
        }
    }
}