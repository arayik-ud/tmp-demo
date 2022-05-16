using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TextCore;
using TMPro;
using Assets;

public class RenderEditorContent : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GetRequest("http://localhost:8080"));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError($"Error: {webRequest.error}");
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError($"HTTP Error: {webRequest.error}");
                    break;
                case UnityWebRequest.Result.Success:
                    var editorContent = JsonUtility.FromJson<EditorContentDto>(webRequest.downloadHandler.text);
                    StartCoroutine(GetTexturesAndRender(editorContent));
                    Debug.Log($"Received text: {webRequest.downloadHandler.text}");
                    break;
            }
        }
    }

    IEnumerator GetTexturesAndRender(EditorContentDto data)
    {
        var textures = new List<Texture2D>();

        foreach (var url in data.images)
        {
            var www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                var texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                textures.Add(texture);
            }
        }

        RenderAsset(textures, data.text);
    }

    void RenderAsset(List<Texture2D> textures, string text)
    {
        var atlas = new Texture2D(12, 12);
        Rect[] texts = atlas.PackTextures(textures.ToArray(), 0);
        var newSprite = ScriptableObject.CreateInstance<TMP_SpriteAsset>();
        var sprites = new List<TMP_Sprite>();

        foreach (var item in textures.Select((value, index) => new { index, value }))
        {
            var texture = item.value;
            var sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

            sprites.Add(new TMP_Sprite
            {
                name = $"sprite-{item.index}",
                sprite = sprite,
                pivot = sprite.pivot,
                scale = 1,
                x = texts[item.index].x * atlas.width,              // set sprite position x based on atlas rect
                y = texts[item.index].y * atlas.height,             // set sprite position y based on atlas rect
                width = texts[item.index].width * atlas.width,      // set sprite width based on atlas rect
                height = texts[item.index].height * atlas.height    // set sprite height based on atlas rect,
            });
        }

        newSprite.name = $"SpriteAsset{atlas.GetHashCode()}";
        newSprite.spriteSheet = atlas;
        newSprite.spriteInfoList = sprites;

        var shader = Shader.Find("TextMeshPro/Sprite");
        var material = new Material(shader) { mainTexture = atlas };
        newSprite.material = material;
        var component = GetComponent<TMP_Text>();
        component.spriteAsset = newSprite;
        component.text = text;

        var rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(185, 5);
        rectTransform.anchoredPosition3D = new Vector3(0, 80, 0);

        for (int i = 0; i < newSprite.spriteCharacterTable.Count; i++)
            newSprite.spriteCharacterTable[i].glyphIndex = (uint)i;

        newSprite.UpdateLookupTables();

        for (var i = 0; i < component.spriteAsset.spriteCharacterTable.Count; i++)
        {
            component.spriteAsset.spriteCharacterTable[i].glyphIndex = (uint)i;
            component.spriteAsset.spriteCharacterTable[i].glyph.metrics = new GlyphMetrics(sprites[i].width, sprites[i].height, 0, sprites[i].height, sprites[i].width);
        }

        component.spriteAsset.UpdateLookupTables();
    }
}
