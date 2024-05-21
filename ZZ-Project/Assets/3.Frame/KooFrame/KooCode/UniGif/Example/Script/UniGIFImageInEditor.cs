// //****************** 代码文件申明 ************************
// //* 文件：UniGIFImageInEditor                                       
// //* 作者：Koo
// //* 创建时间：2024/05/20 20:43:13 星期一
// //* 功能：nothing
// //*****************************************************
//
// using System.Collections;
// using System.Collections.Generic;
// using System.IO;
// using Unity.EditorCoroutines.Editor;
// using UnityEngine;
// using UnityEngine.Networking;
// using UnityEngine.UIElements;
//
// namespace KooFrame
// {
//     public class UniGIFImageInEditor : VisualElement
//     {
//         public enum State
//         {
//             None,
//             Loading,
//             Ready,
//             Playing,
//             Pause,
//         }
//
//         private Image image;
//
//         // Textures filter mode
//         private FilterMode m_filterMode = FilterMode.Point;
//
//         // Textures wrap mode
//         private TextureWrapMode m_wrapMode = TextureWrapMode.Clamp;
//
//         // Load from url on start
//         [SerializeField] private bool m_loadOnStart;
//
//         // GIF image url (WEB or StreamingAssets path)
//         [SerializeField] private string m_loadOnStartUrl;
//
//         // Debug log flag
//         [SerializeField] private bool m_outputDebugLog;
//
//         // Decoded GIF texture list
//         private List<global::UniGif.GifTexture> m_gifTextureList;
//
//         // Delay time
//         private float m_delayTime;
//
//         // Texture index
//         private int m_gifTextureIndex;
//
//         // loop counter
//         private int m_nowLoopCount;
//
//         /// <summary>
//         /// Now state
//         /// </summary>
//         public State nowState { get; private set; }
//
//         /// <summary>
//         /// Animation loop count (0 is infinite)
//         /// </summary>
//         public int loopCount { get; private set; }
//
//
//         public void Init(KooCodeWindow window)
//         {
//             if (image == null)
//             {
//                 image = new Image();
//                 this.Add(image);
//             }
//
//             window.OnWindowUpdate += Update;
//             window.OnClose += OnDestroy;
//
//             if (m_loadOnStart)
//             {
//                 SetGifFromUrl(m_loadOnStartUrl);
//             }
//         }
//
//         private void OnDestroy(KooCodeWindow window)
//         {
//             window.OnWindowUpdate -= Update;
//             Clear();
//         }
//
//         private void Update()
//         {
//             switch (nowState)
//             {
//                 case State.None:
//                     break;
//
//                 case State.Loading:
//                     break;
//
//                 case State.Ready:
//                     break;
//
//                 case State.Playing:
//                     if (image == null || m_gifTextureList == null || m_gifTextureList.Count <= 0)
//                     {
//                         return;
//                     }
//
//                     if (m_delayTime > Time.time)
//                     {
//                         return;
//                     }
//
//                     // Change texture
//                     m_gifTextureIndex++;
//                     if (m_gifTextureIndex >= m_gifTextureList.Count)
//                     {
//                         m_gifTextureIndex = 0;
//
//                         if (loopCount > 0)
//                         {
//                             m_nowLoopCount++;
//                             if (m_nowLoopCount >= loopCount)
//                             {
//                                 Stop();
//                                 return;
//                             }
//                         }
//                     }
//
//                     image.image = m_gifTextureList[m_gifTextureIndex].m_texture2d;
//                     m_delayTime = Time.time + m_gifTextureList[m_gifTextureIndex].m_delaySec;
//                     break;
//
//                 case State.Pause:
//                     break;
//
//                 default:
//                     break;
//             }
//         }
//
//         /// <summary>
//         /// Set GIF texture from url
//         /// </summary>
//         /// <param name="url">GIF image url (WEB or StreamingAssets path)</param>
//         /// <param name="autoPlay">Auto play after decode</param>
//         public void SetGifFromUrl(string url, bool autoPlay = true)
//         {
//             EditorCoroutineUtility.StartCoroutine(SetGifFromUrlCoroutine(url, autoPlay), this);
//         }
//
//         /// <summary>
//         /// Set GIF texture from url
//         /// </summary>
//         /// <param name="url">GIF image url (WEB or StreamingAssets path)</param>
//         /// <param name="autoPlay">Auto play after decode</param>
//         /// <returns>IEnumerator</returns>
//         public IEnumerator SetGifFromUrlCoroutine(string url, bool autoPlay = true)
//         {
//             if (string.IsNullOrEmpty(url))
//             {
//                 Debug.LogError("URL is nothing.");
//                 yield break;
//             }
//
//             if (nowState == State.Loading)
//             {
//                 Debug.LogWarning("Already loading.");
//                 yield break;
//             }
//
//             nowState = State.Loading;
//
//             string path;
//             if (url.StartsWith("http"))
//             {
//                 // from WEB
//                 path = url;
//                 Debug.Log(path);
//             }
//             else
//             {
//                 // from StreamingAssets
//                 path = Path.Combine("file:///" + Application.streamingAssetsPath, url);
//             }
//
//             // Load file
//             UnityWebRequest request = UnityWebRequest.Get(path);
//             request.SendWebRequest();
//             while (!request.isDone)
//             {
//                 yield return null;
//             }
//
//             if (string.IsNullOrEmpty(request.error) == false)
//             {
//                 Debug.LogError("File load error.\n" + request.error + "\npath: " + path);
//                 nowState = State.None;
//                 yield break;
//             }
//
//             Clear();
//             nowState = State.Loading;
//
//             // Get GIF textures
//             yield return EditorCoroutineUtility.StartCoroutine(global::UniGif.GetTextureListCoroutine(
//                 request.downloadHandler.data,
//                 (gifTexList, loopCount, width, height) =>
//                 {
//                     request.Dispose();
//                     if (gifTexList != null)
//                     {
//                         m_gifTextureList = gifTexList;
//                         this.loopCount = loopCount;
//                         this.style.width = width;
//                         this.style.height = height;
//                         nowState = State.Ready;
//
//                         //m_imgAspectCtrl.FixAspectRatio(width, height);
//
//                         if (autoPlay)
//                         {
//                             Play();
//                         }
//                     }
//                     else
//                     {
//                         Debug.LogError("Gif texture get error.");
//                         nowState = State.None;
//                     }
//                 },
//                 m_filterMode, m_wrapMode, m_outputDebugLog), this);
//         }
//
//         /// <summary>
//         /// Clear GIF texture
//         /// </summary>
//         public void Clear()
//         {
//             if (image != null)
//             {
//                 image.image = null;
//             }
//
//             if (m_gifTextureList != null)
//             {
//                 for (int i = 0; i < m_gifTextureList.Count; i++)
//                 {
//                     if (m_gifTextureList[i] != null)
//                     {
//                         if (m_gifTextureList[i].m_texture2d != null)
//                         {
//                             Object.Destroy(m_gifTextureList[i].m_texture2d);
//                             m_gifTextureList[i].m_texture2d = null;
//                         }
//
//                         m_gifTextureList[i] = null;
//                     }
//                 }
//
//                 m_gifTextureList.Clear();
//                 m_gifTextureList = null;
//             }
//
//             nowState = State.None;
//         }
//
//         /// <summary>
//         /// Play GIF animation
//         /// </summary>
//         public void Play()
//         {
//             if (nowState != State.Ready)
//             {
//                 Debug.LogWarning("State is not READY.");
//                 return;
//             }
//
//             if (image == null || m_gifTextureList == null || m_gifTextureList.Count <= 0)
//             {
//                 Debug.LogError("Raw Image or GIF Texture is nothing.");
//                 return;
//             }
//
//             nowState = State.Playing;
//             image.image = m_gifTextureList[0].m_texture2d;
//             m_delayTime = Time.time + m_gifTextureList[0].m_delaySec;
//             m_gifTextureIndex = 0;
//             m_nowLoopCount = 0;
//         }
//
//         /// <summary>
//         /// Stop GIF animation
//         /// </summary>
//         public void Stop()
//         {
//             if (nowState != State.Playing && nowState != State.Pause)
//             {
//                 Debug.LogWarning("State is not Playing and Pause.");
//                 return;
//             }
//
//             nowState = State.Ready;
//         }
//
//         /// <summary>
//         /// Pause GIF animation
//         /// </summary>
//         public void Pause()
//         {
//             if (nowState != State.Playing)
//             {
//                 Debug.LogWarning("State is not Playing.");
//                 return;
//             }
//
//             nowState = State.Pause;
//         }
//
//         /// <summary>
//         /// Resume GIF animation
//         /// </summary>
//         public void Resume()
//         {
//             if (nowState != State.Pause)
//             {
//                 Debug.LogWarning("State is not Pause.");
//                 return;
//             }
//
//             nowState = State.Playing;
//         }
//     }
// }