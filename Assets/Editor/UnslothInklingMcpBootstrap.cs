using UnityEditor;

namespace UnslothInklingUnityTest.Editor
{
    /// <summary>
    /// On editor load, opt this machine into MCP for Unity's HTTP transport
    /// and auto-start the local bridge so LM Studio can connect at
    /// http://127.0.0.1:8080/mcp without walking through the setup wizard.
    /// Pref keys match MCPForUnity.Editor.Constants.EditorPrefKeys.
    /// </summary>
    [InitializeOnLoad]
    internal static class UnslothInklingMcpBootstrap
    {
        static UnslothInklingMcpBootstrap()
        {
            EditorPrefs.SetBool("MCPForUnity.UseHttpTransport", true);
            EditorPrefs.SetBool("MCPForUnity.AutoStartOnLoad", true);
            EditorPrefs.SetBool("MCPForUnity.SetupCompleted", true);
            EditorPrefs.SetBool("MCPForUnity.SetupDismissed", true);
        }
    }
}
