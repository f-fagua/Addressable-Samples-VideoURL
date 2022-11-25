<h1>Addressables Samples VideoURL</h1>

<p>By default, each addressable entry loads the corresponding asset as it is. This is the workflow that 99.9999% of users expect. However, there are cases in which developers need an edited version of the asset (HD or SD versions of a sprite) or even a completely different object. For example, regarding video assets, there are commonly used 3rd party plugins that do not take the packed video within the bundles; instead, they need the video bytes or the file URL to playback the video.</p>

<p>This example is a workaround for the above case. Before starting the build, it creates a scriptable object with the asset path and produces a customizable URL. The scriptable object replaces the Addressable video entry but keeps the same key, <code>Assets/Videos/My Video.mp4</code>. Thus, in runtime, users can load this SO and ask for its URL.</p>

<h2>What is the purpose of this example?</h2>

<p>To show the packaging flexibility to fulfill use cases beyond the original capabilities.</p> 

<p>Also, teach users how to create custom build scripts that edit the Addressables entries.</p>

<p>In this case, developers will see how to replace entries on build time and restore the previous state after the build ends.</p>

<h2>Repository content</h2>

<p>A basic project with a custom build script set that replaces any video within the build for scriptable objects with the video URLs.</p>

<h2>How to use it</h2>

<ol>
	<li>Import the project.</li>
	<li>Make the Addressables build.</li>
	<li>Set the Addressables play mode to <code>Use Existing Build</code>.</li>
	<li>Open the <code>SampleScene</code>.</li>
	<li>Hit play and check the console. The loading script should print the video URL</li>
</ol>