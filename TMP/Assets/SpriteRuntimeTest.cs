using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Assets;
using System.Linq;

public class SpriteRuntimeTest : MonoBehaviour
{
    // WIP: Multiple Image Support
    void Start()
    {
        var data = new EditorContentDTO
        {
            text = @"<align=left><color=#000><mark=#ffff00aa>Test1</mark></color></align>
<align=right><color=#000>Test 2</color></align>
<sprite index=0> ",
            imageUrl = "https://cdn.pixabay.com/photo/2015/12/01/20/28/road-1072821__340.jpg",
            scale = 78
        };
        //ProcessData(JsonUtility.ToJson(data));
        StartCoroutine(GetTextureTest());

    }

    void ProcessData(string data)
    {
        //var editorContent = JsonUtility.FromJson<EditorContentDTO>(data);
        //StartCoroutine(GetTexture(editorContent));
    }

    IEnumerator GetTextureTest()
    {
        var urls = new List<string>
        {
            "https://cdn.pixabay.com/photo/2015/12/01/20/28/road-1072821__340.jpg",
            "https://thumbs.dreamstime.com/b/environment-earth-day-hands-trees-growing-seedlings-bokeh-green-background-female-hand-holding-tree-nature-field-gra-130247647.jpg",
            //"https://images.unsplash.com/photo-1610878180933-123728745d22?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxzZWFyY2h8MXx8Y2FuYWRhJTIwbmF0dXJlfGVufDB8fDB8fA%3D%3D&w=1000&q=80"
        };

        var textures = new List<Texture2D>();

        foreach (var url in urls)
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

        TestRenderAsset(textures, "<sprite name=sprite-0>", 70);
    }

    void TestRenderAsset(List<Texture2D> textures, string text, int scale)
    {
        var atlas = new Texture2D(12,12);
        atlas.PackTextures(textures.ToArray(), 0);
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
                width = texture.width,
                height = texture.height,
                scale = (float)(scale / 2.631578947368421)
            });
        }

        Debug.Log(sprites.Count);

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
        rectTransform.anchoredPosition3D = new Vector3(0, 32, 88);
        newSprite.UpdateLookupTables();
    }

    //void RenderAsset(Texture2D texture, string text, int scale)
    //{
    //    var sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    //    var newSprite = ScriptableObject.CreateInstance<TMP_SpriteAsset>();

    //    newSprite.name = $"SpriteAsset{sprite.GetHashCode()}";
    //    newSprite.spriteSheet = texture;
    //    var tmpSprite = new TMP_Sprite
    //    {
    //        name = sprite.name,
    //        sprite = sprite,
    //        pivot = sprite.pivot,
    //        width = texture.width,
    //        height = texture.height,
    //        scale = (float)(scale / 2.631578947368421)
    //    };

    //    var sprites = new List<TMP_Sprite>
    //    {
    //        tmpSprite
    //    };

    //    newSprite.spriteInfoList = sprites;

    //    var shader = Shader.Find("TextMeshPro/Sprite");
    //    var material = new Material(shader) { mainTexture = texture };
    //    newSprite.material = material;
    //    var component = GetComponent<TMP_Text>();
    //    component.spriteAsset = newSprite;
    //    component.text = text;

    //    var rectTransform = GetComponent<RectTransform>();
    //    rectTransform.sizeDelta = new Vector2(185, 5);
    //    rectTransform.anchoredPosition3D = new Vector3(0, 32, 88);
    //}

    //IEnumerator GetTexture(EditorContentDTO editorContent)
    //{
    //    var www = UnityWebRequestTexture.GetTexture(editorContent.imageUrl);
    //    yield return www.SendWebRequest();

    //    if (www.result != UnityWebRequest.Result.Success)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        var texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
    //        RenderAsset(texture, editorContent.text, editorContent.scale);
    //    }
    //}
}
