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
    private static ImageAnimationManager instance = null;

    private GameObject overlayObject;
    private RawImage image;
    private bool isPlaying = false;

    private List<Texture2D> textures;
    private int textureIndex;
    private float delayTime;

    public static ImageAnimationManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("ImageAnimationManager").AddComponent<ImageAnimationManager>();
                var prefab = Resources.Load<GameObject>("Prefabs/AnimationManager");
                instance.overlayObject = GameObject.Instantiate(prefab);
                instance.overlayObject.transform.SetParent(instance.transform);

                instance.image = instance.overlayObject.transform.Find("Panel/RawImage").GetComponent<RawImage>();

                DontDestroyOnLoad(instance.gameObject);
                instance.overlayObject.SetActive(false);
            }

            return instance;
        }
    }

    public void ShowAnimation(int id)
    {
        if (isPlaying)
        {
            return;
        }

        isPlaying = true;

        StartCoroutine(InitAnimation(id.ToString()));
    }

    private IEnumerator InitAnimation(string id)
    {
        overlayObject.SetActive(true);

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
        delayTime = Time.time + (1/30f);

        image.SetNativeSize();
        image.transform.localScale = new Vector3(0.8f, 0.8f);
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
                isPlaying = false;
                overlayObject.SetActive(false);
            }

            image.texture = textures[textureIndex];
            delayTime = Time.time + (1 / 30f);
        }
    }

    private void OnDestroy()
    {
        Clear();
    }

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

    //public UniGifImage gifImage;

    //public static ImageAnimationManager instance = null;

    //private bool isBusy = false;

    //private void Awake()
    //{
    //    if (instance != null && instance != this)
    //    {
    //        return;
    //    }

    //    instance = this;
    //    DontDestroyOnLoad(gameObject);

    //    gameObject.SetActive(false);
    //}

    //public void ShowAnimation(string path)
    //{
    //    if (isBusy || string.IsNullOrEmpty(path))
    //    {
    //        return;
    //    }

    //    ToggleVisibility();

    //    StartCoroutine(ViewGifCoroutine(path));
    //}

    //private IEnumerator ViewGifCoroutine(string path)
    //{
    //    yield return StartCoroutine(gifImage.SetGifFromUrlCoroutine(path));
    //    ToggleVisibility();
    //}

    //private void ToggleVisibility()
    //{
    //    isBusy = !isBusy;
    //    gameObject.SetActive(isBusy);
    //}
}
