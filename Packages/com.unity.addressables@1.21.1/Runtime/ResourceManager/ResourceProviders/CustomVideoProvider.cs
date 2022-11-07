using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace UnityEngine.ResourceManagement.ResourceProviders
{
    /// <summary>
    /// Provides raw text from a local or remote URL.
    /// </summary>
    [DisplayName("Video URL Provider")]
    public class CustomVideoProvider : ResourceProviderBase
    {
        /// <summary>
        /// Controls whether errors are logged - this is disabled when trying to load from the local cache since failures are expected
        /// </summary>
        public bool IgnoreFailures { get; set; }

        internal class InternalOp
        {
            CustomVideoProvider m_Provider;
            private bool m_Complete;

            ProvideHandle m_PI;

            private float GetPercentComplete()
            {
                return 0.0f;
            }
            
            public void Start(ProvideHandle provideHandle, CustomVideoProvider rawProvider)
            {
                m_PI = provideHandle;
                m_PI.SetWaitForCompletionCallback(WaitForCompletionHandler);
                provideHandle.SetProgressCallback(GetPercentComplete);
                m_Provider = rawProvider;
                
                var videoURL = m_PI.ResourceManager.TransformInternalId(m_PI.Location);

                if (string.IsNullOrEmpty(videoURL))
                {
                    m_PI.Complete(videoURL, false, 
                        new Exception($"Unable to find the video URL from location {m_PI.Location.PrimaryKey}."));
                    m_Complete = true;
                }
                else
                {
                    m_PI.Complete(videoURL, true, null);
                    m_Complete = true;
                }

                bool WaitForCompletionHandler()
                {
                    return m_Complete;
                }
            }
        }

        /// <summary>
        /// Provides raw text data from the location.
        /// </summary>
        /// <param name="provideHandle">The data needed by the provider to perform the load.</param>
        public override void Provide(ProvideHandle provideHandle)
        {
            new InternalOp().Start(provideHandle, this);
        }
    }
}