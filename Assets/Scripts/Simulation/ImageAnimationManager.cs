using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimationManager : MonoBehaviour
{
    private GameObject overlayObject;
    private RawImage image;
    private bool isPlaying = false;

    private List<Texture2D> textures;
    private Transform targetTransform;
    private int textureIndex;
    private float delayTime;
    private bool isTransformHidden = false;
    private bool replaceOnEnd = false;
    private Func<bool> doActionOnEnd = null;

    private static EZObjectPools.EZObjectPool objectPool = null;

    public static void CreateAnimation(int id, Transform transform, Action endAction = null, bool replaceImage = true, bool autoHide = true)
    {
        if (objectPool == null)
        {
            //var obj = new GameObject("ImageAnimationManager").AddComponent<ImageAnimationManager>();
            var prefab = Resources.Load<GameObject>("Prefabs/AnimationManager");
            //instance.overlayObject = GameObject.Instantiate(prefab);
            //instance.overlayObject.transform.SetParent(instance.transform);

            //instance.image = instance.overlayObject.transform.Find("Panel/RawImage").GetComponent<RawImage>();

            objectPool = EZObjectPools.EZObjectPool.CreateObjectPool(prefab, "AnimObjectPool", 2, true, true, true);
        }

        GameObject obj;
        if (objectPool.TryGetNextObject(Vector3.zero, Quaternion.identity, out obj))
        {
            //var instance = new ImageAnimationManager();
            obj.AddComponent<ImageAnimationManager>();
            var instance = obj.GetComponent<ImageAnimationManager>();
            instance.image = obj.transform.Find("Panel/RawImage").GetComponent<RawImage>();
            instance.overlayObject = obj;
            instance.doActionOnEnd = () =>
            {
                endAction?.Invoke();
                return true;
            };
            instance.ShowAnimation(id, transform, replaceImage, autoHide);
        }
        else
        {
            Debug.LogWarning("Failed to generate animation for id " + id);
        }
    }

    public static void CreateLoopingAnimation(int id, Transform transform, Func<bool> onEndLoop, bool replaceImage = true, bool autoHide = true)
    {
        if (objectPool == null)
        {
            //var obj = new GameObject("ImageAnimationManager").AddComponent<ImageAnimationManager>();
            var prefab = Resources.Load<GameObject>("Prefabs/AnimationManager");
            //instance.overlayObject = GameObject.Instantiate(prefab);
            //instance.overlayObject.transform.SetParent(instance.transform);

            //instance.image = instance.overlayObject.transform.Find("Panel/RawImage").GetComponent<RawImage>();

            objectPool = EZObjectPools.EZObjectPool.CreateObjectPool(prefab, "AnimObjectPool", 2, true, true, true);
        }

        GameObject obj;
        if (objectPool.TryGetNextObject(Vector3.zero, Quaternion.identity, out obj))
        {
            //var instance = new ImageAnimationManager();
            obj.AddComponent<ImageAnimationManager>();
            var instance = obj.GetComponent<ImageAnimationManager>();
            instance.image = obj.transform.Find("Panel/RawImage").GetComponent<RawImage>();
            instance.overlayObject = obj;
            instance.doActionOnEnd = onEndLoop;
            instance.ShowAnimation(id, transform, replaceImage, autoHide);
        }
        else
        {
            Debug.LogWarning("Failed to generate animation for id " + id);
        }
    }

    public void ShowAnimation(int id, Transform transform, bool replaceImage = true, bool autoHide = true)
    {
        isPlaying = true;
        isTransformHidden = autoHide;
        replaceOnEnd = replaceImage;

        StartCoroutine(InitAnimation(id.ToString(), transform));
    }

    private IEnumerator InitAnimation(string id, Transform transform)
    {
        overlayObject.SetActive(true);
        targetTransform = transform;

        try
        {
            var res = Resources.LoadAll("Animations/" + id, typeof(Texture2D));

            if (textures == null)
            {
                textures = new List<Texture2D>();
            }
            textures.Clear();

            foreach (var item in res)
            {
                textures.Add((Texture2D)item);
            }

            Play();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        yield break;
    }

    private void Play()
    {
        if (image == null || textures == null || textures.Count <= 0)
        {
            Debug.LogError("Unable to load animation. No textures found");
            return;
        }

        image.texture = textures[0];
        textureIndex = 0;
        delayTime = Time.time + (1 / 30f);

        image.SetNativeSize();
        image.transform.localScale = new Vector3(0.5f, 0.5f);

        var pos = Camera.main.WorldToScreenPoint(targetTransform.position);
        image.transform.position = pos;

        if (isTransformHidden)
        {
            targetTransform.GetComponent<Renderer>().enabled = false;
        }
    }

    private void Update()
    {
        if (isPlaying)
        {
            Time.timeScale = 1.0f;
            if (textures == null || textures.Count <= 0 || delayTime > Time.time)
            {
                return;
            }

            textureIndex++;
            if (textureIndex >= textures.Count)
            {
                textureIndex = 0;

                if (!doActionOnEnd())
                {
                    image.texture = textures[textureIndex];
                    delayTime = Time.time + (1 / 30f);
                }
                else
                {

                    isPlaying = false;
                    overlayObject.SetActive(false);

                    if (isTransformHidden)
                    {
                        targetTransform.GetComponent<Renderer>().enabled = true;
                    }

                    if (replaceOnEnd)
                    {
                        var sprite = Sprite.Create(textures[textures.Count - 1],
                            new Rect(0, 0,
                            image.texture.width,
                            image.texture.height),
                            new Vector2(0.5f, 0.5f),
                            80);

                        targetTransform.GetComponent<DraggableObjectBehavior>().MixtureItem.icon = sprite;
                        targetTransform.GetComponent<DropZoneObjectHandler>().GetComponent<DropZoneObjectHandler>().SetIcon(sprite);
                    }
                }
            }
            else
            {
                image.texture = textures[textureIndex];
                delayTime = Time.time + (1 / 30f);
            }
        }
    }

    //private void OnDestroy()
    //{
    //    Clear();
    //}

    private void Clear()
    {
        if (image != null)
        {
            image.texture = null;
        }

        if (textures != null)
        {
            foreach (var item in textures)
            {
                Destroy(item);
            }

            textures.Clear();
            textures = null;
        }

        isPlaying = false;
    }
}
