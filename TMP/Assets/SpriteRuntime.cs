using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Assets;

public class SpriteRuntime : MonoBehaviour
{
    void Start()
    {
        // For runtime testing
        // For tags, please refer to - http://digitalnativestudios.com/textmeshpro/docs/rich-text/
        var data = new EditorContentDTO
        {
            text = @"<align=left><color=#00b3d6><b>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris nec tempor nibh. Nulla elementum mi vel ex maximus sodales. Phasellus non tortor sit amet orci pulvinar dignissim. Duis pharetra, ligula nec lobortis tincidunt, turpis sem consequat ex, sit amet convallis ligula lectus id nisi. Praesent posuere augue risus, et tincidunt magna pellentesque a. Integer pulvinar, elit ac gravida dignissim, nulla tellus faucibus ipsum, eu mattis arcu quam at nisi.</b></color><color=#00b3d6> </color><color=#000><mark=#ffff00aa>Mauris nec elementum dui. Nullam nec pellentesque nulla. Donec eu laoreet metus.</mark></color><color=#00b3d6> </color><color=#000><i>Nam sed erat eu sapien hendrerit euismod convallis vitae erat.</i></color><color=#00b3d6> </color><color=#000><u>Curabitur vel velit diam. Morbi eleifend lectus et nulla varius, vel interdum neque rutrum.</u></color></align>
<align=left></align>
<align=right><color=#000>Praesent quis tortor at risus vehicula vehicula. Curabitur sodales fermentum felis vel blandit. Nunc semper est sit amet ipsum ultricies rutrum in in tellus. Donec consequat fermentum lorem nec tempus. Nam blandit sem nec vestibulum vestibulum.</color></align>
<align=left><color=#000><style=H1>Heading 1</style></color></align>
<align=left><color=#000><style=H2>Heading 2</style></color></align>
<align=left><color=#000> </color></align>
<align=right><color=#c45a03><i>Donec et nisl et sapien pharetra euismod at eleifend diam. Sed lacus ipsum, vulputate eu nulla eget, malesuada tincidunt felis. Nulla pretium mauris neque, a fermentum magna placerat vitae. Aliquam auctor sapien a ligula pretium scelerisque.</i></color><color=#c45a03> Phasellus vitae sem augue. Pellentesque accumsan suscipit efficitur. Curabitur nec dolor vel purus scelerisque gravida non ut diam. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae;</color></align>
<pos=22%><sprite index=0>",
            imageUrl = "https://cdn.pixabay.com/photo/2015/12/01/20/28/road-1072821__340.jpg",
            scale = 78
        };
        ProcessData(JsonUtility.ToJson(data));
    }

    void ProcessData(string data)
    {
        var editorContent = JsonUtility.FromJson<EditorContentDTO>(data);
        StartCoroutine(GetTexture(editorContent));
    }

    void RenderAsset(Texture2D texture, string text, int scale)
    {
        var sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        var newSprite = ScriptableObject.CreateInstance<TMP_SpriteAsset>();

        newSprite.name = $"SpriteAsset{sprite.GetHashCode()}";
        newSprite.spriteSheet = texture;
        var tmpSprite = new TMP_Sprite
        {
            name = sprite.name,
            sprite = sprite,
            pivot = sprite.pivot,
            width = texture.width,
            height = texture.height,
            scale = (float)(scale / 1.989795918367347)
        };

        var sprites = new List<TMP_Sprite>
        {
            tmpSprite
        };

        newSprite.spriteInfoList = sprites;

        var shader = Shader.Find("TextMeshPro/Sprite");
        var material = new Material(shader) { mainTexture = texture };
        newSprite.material = material;
        var component = GetComponent<TMP_Text>();
        component.spriteAsset = newSprite;
        component.text = text;

        var rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(245.35f, 46);
        rectTransform.anchoredPosition3D = new Vector3(0, 32, 88);
    }

    IEnumerator GetTexture(EditorContentDTO editorContent)
    {
        var www = UnityWebRequestTexture.GetTexture(editorContent.imageUrl);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            var texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            RenderAsset(texture, editorContent.text, editorContent.scale);
        }
    }
}
