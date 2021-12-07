## TMP Unity Project

The project contain two directories:
`TMP` and `TMP Build`

`TMP` is the main Unity Project, demonstrating the rendering of a WYSIWYG editor content to TMP (TextMeshPro) format.
`TMP_Build` is the WebGL build of the `TMP` project.

To launch

`TMP`: Add the project to the Unity Hub and open with Unity Editor. `SpriteRuntime.cs` file contains the logic for conversion.
`TMP_Build`: Open the project with VS Code and using the `LiveServer` extension, launch it (it will be launched on http://localhost:5500 by default). The `editor-content.json`, located at the root contains the data (`EditorContentDTO`) that is used to render the content.