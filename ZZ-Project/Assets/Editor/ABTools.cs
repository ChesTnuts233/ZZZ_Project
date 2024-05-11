using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class ABTools : EditorWindow
{
    private int nowSelIndex = 0;
    private string[] targetStrings = new string[] { "PC", "IOS", "Android" };
    //��Դ������Ĭ��IP��ַ
    private string serverIP = "ftp://192.168.31.178";

    [MenuItem("AB������/�򿪹��ߴ���")]
    private static void OpenWindow()
    {
        //��ȡһ��ABTools �༭�����ڶ���
        ABTools windown = EditorWindow.GetWindowWithRect(typeof(ABTools), new Rect(0, 0, 350, 220)) as ABTools;
        windown.Show();
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 150, 15), "ƽ̨ѡ��");
        //ҳǩ��ʾ �Ǵ�������ȡ���ַ�����������ʾ ���� ��Ҫ�ı䵱ǰѡ�е�����
        nowSelIndex = GUI.Toolbar(new Rect(10, 30, 250, 20), nowSelIndex, targetStrings);
        //��Դ������IP��ַ����
        GUI.Label(new Rect(10, 60, 150, 15), "��Դ��������ַ");
        serverIP = GUI.TextField(new Rect(10, 80, 150, 20), serverIP);
        //�����Ա��ļ� ��ť
        if(GUI.Button(new Rect(10, 110, 100, 40), "�����Ա��ļ�"))
            CreateABCompareFile();
        //����Ĭ����Դ��StreamingAssets ��ť
        if (GUI.Button(new Rect(115, 110, 225, 40), "����Ĭ����Դ��StreamingAssets"))
            MoveABToStreamingAssets();
        //�ϴ�AB���ͶԱ��ļ� ��ť
        if (GUI.Button(new Rect(10, 160, 330, 40), "�ϴ�AB���ͶԱ��ļ�"))
            UploadAllABFile();
    }

    //����AB���Ա��ļ�
    private void CreateABCompareFile()
    {
        //��ȡ�ļ�����Ϣ
        //Ҫ����ѡ���ƽ̨��ȡ��Ӧƽ̨�ļ����µ����� �����жԱ��ļ�������
        DirectoryInfo directory = Directory.CreateDirectory(Application.dataPath + "/ArtRes/AB/" + targetStrings[nowSelIndex]);
        //��ȡ��Ŀ¼�µ������ļ���Ϣ
        FileInfo[] fileInfos = directory.GetFiles();

        //���ڴ洢��Ϣ�� �ַ���
        string abCompareInfo = "";

        foreach (FileInfo info in fileInfos)
        {
            //û�к�׺�� ����AB�� ����ֻ��ҪAB������Ϣ
            if (info.Extension == "")
            {
                //Debug.Log("�ļ�����" + info.Name);
                //ƴ��һ��AB������Ϣ
                abCompareInfo += info.Name + " " + info.Length + " " + GetMD5(info.FullName);
                //��һ���ָ����ֿ���ͬ�ļ�֮�����Ϣ
                abCompareInfo += '|';
            }
            //Debug.Log("**********************");
            //Debug.Log("�ļ�����" + info.Name);
            //Debug.Log("�ļ�·����" + info.FullName);
            //Debug.Log("�ļ���׺��" + info.Extension);
            //Debug.Log("�ļ���С��" + info.Length);
        }
        //��Ϊѭ����Ϻ� ���������һ�� | ���� ���� ����ȥ��
        abCompareInfo = abCompareInfo.Substring(0, abCompareInfo.Length - 1);

        //Debug.Log(abCompareInfo);

        //�洢ƴ�Ӻõ� AB����Դ��Ϣ
        File.WriteAllText(Application.dataPath + "/ArtRes/AB/" + targetStrings[nowSelIndex] + "/ABCompareInfo.txt", abCompareInfo);
        //ˢ�±༭��
        AssetDatabase.Refresh();

        Debug.Log("AB���Ա��ļ����ɳɹ�");
    }
    //��ȡ�ļ�MD5��
    private string GetMD5(string filePath)
    {
        //���ļ���������ʽ��
        using (FileStream file = new FileStream(filePath, FileMode.Open))
        {
            //����һ��MD5���� ��������MD5��
            MD5 md5 = new MD5CryptoServiceProvider();
            //����API �õ����ݵ�MD5�� 16���ֽ� ����
            byte[] md5Info = md5.ComputeHash(file);

            //�ر��ļ���
            file.Close();

            //��16���ֽ�ת��Ϊ 16���� ƴ�ӳ��ַ��� Ϊ�˼�Сmd5��ĳ���
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < md5Info.Length; i++)
                sb.Append(md5Info[i].ToString("x2"));

            return sb.ToString();
        }
    }

    //��ѡ����Դ�ƶ���StreamingAssets�ļ�����
    private void MoveABToStreamingAssets()
    {
        //ͨ���༭��Selection���еķ��� ��ȡ��Project������ѡ�е���Դ 
        UnityEngine.Object[] selectedAsset = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
        //���һ����Դ��û��ѡ�� ��û�б�Ҫ���������߼���
        if (selectedAsset.Length == 0)
            return;
        //����ƴ�ӱ���Ĭ��AB����Դ��Ϣ���ַ���
        string abCompareInfo = "";
        //����ѡ�е���Դ����
        foreach (UnityEngine.Object asset in selectedAsset)
        {
            //ͨ��Assetdatabase�� ��ȡ ��Դ��·��
            string assetPath = AssetDatabase.GetAssetPath(asset);
            //��ȡ·�����е��ļ��� ������Ϊ StreamingAssets�е��ļ���
            string fileName = assetPath.Substring(assetPath.LastIndexOf('/'));

            //�ж��Ƿ���.���� ����� ֤���к�׺ ������
            if (fileName.IndexOf('.') != -1)
                continue;
            //�㻹�����ڿ���֮ǰ ȥ��ȡȫ·�� Ȼ��ͨ��FIleInfoȥ��ȡ��׺���ж� �������ӵ�׼ȷ

            //����AssetDatabase�е�API ��ѡ���ļ� ���Ƶ�Ŀ��·��
            AssetDatabase.CopyAsset(assetPath, "Assets/StreamingAssets" + fileName);

            //��ȡ������StreamingAssets�ļ����е��ļ���ȫ����Ϣ
            FileInfo fileInfo = new FileInfo(Application.streamingAssetsPath + fileName);
            //ƴ��AB����Ϣ���ַ�����
            abCompareInfo += fileInfo.Name + " " + fileInfo.Length + " " + CreateABCompare.GetMD5(fileInfo.FullName);
            //��һ�����Ÿ������AB����Ϣ
            abCompareInfo += "|";
        }
        //ȥ�����һ��|���� Ϊ��֮�����ַ�������
        abCompareInfo = abCompareInfo.Substring(0, abCompareInfo.Length - 1);
        //������Ĭ����Դ�ĶԱ���Ϣ �����ļ�
        File.WriteAllText(Application.streamingAssetsPath + "/ABCompareInfo.txt", abCompareInfo);
        //ˢ�´���
        AssetDatabase.Refresh();
    }

    //�ϴ�AB���ļ���������
    private void UploadAllABFile()
    {
        //��ȡ�ļ�����Ϣ
        DirectoryInfo directory = Directory.CreateDirectory(Application.dataPath + "/ArtRes/AB/" + targetStrings[nowSelIndex] + "/");
        //��ȡ��Ŀ¼�µ������ļ���Ϣ
        FileInfo[] fileInfos = directory.GetFiles();

        foreach (FileInfo info in fileInfos)
        {
            //û�к�׺�� ����AB�� ����ֻ��ҪAB������Ϣ
            //������Ҫ��ȡ ��Դ�Ա��ļ� ��ʽ��txt�����ļ����� ֻ�жԱ��ļ��ĸ�ʽ����txt ���Կ��������жϣ�
            if (info.Extension == "" ||
                info.Extension == ".txt")
            {
                //�ϴ����ļ�
                FtpUploadFile(info.FullName, info.Name);
            }
        }
    }
    //�첽�ϴ��ļ�
    private async void FtpUploadFile(string filePath, string fileName)
    {
        await Task.Run(() =>
        {
            try
            {
                //1.����һ��FTP���� �����ϴ�
                FtpWebRequest req = FtpWebRequest.Create(new Uri( serverIP + "/AB/" + targetStrings[nowSelIndex] + "/" + fileName)) as FtpWebRequest;
                //2.����һ��ͨ��ƾ֤ ���������ϴ�
                NetworkCredential n = new NetworkCredential("MrTang", "MrTang123");
                req.Credentials = n;
                //3.��������
                //  ���ô���Ϊnull
                req.Proxy = null;
                //  ������Ϻ� �Ƿ�رտ�������
                req.KeepAlive = false;
                //  ��������-�ϴ�
                req.Method = WebRequestMethods.Ftp.UploadFile;
                //  ָ����������� 2����
                req.UseBinary = true;
                //4.�ϴ��ļ�
                //  ftp��������
                Stream upLoadStream = req.GetRequestStream();
                //  ��ȡ�ļ���Ϣ д���������
                using (FileStream file = File.OpenRead(filePath))
                {
                    //һ��һ����ϴ�����
                    byte[] bytes = new byte[2048];
                    //����ֵ �����ȡ�˶��ٸ��ֽ�
                    int contentLength = file.Read(bytes, 0, bytes.Length);

                    //ѭ���ϴ��ļ��е�����
                    while (contentLength != 0)
                    {

                        //д�뵽�ϴ�����
                        upLoadStream.Write(bytes, 0, contentLength);
                        //д���ٶ�
                        contentLength = file.Read(bytes, 0, bytes.Length);
                    }

                    //ѭ����Ϻ� ֤���ϴ�����
                    file.Close();
                    upLoadStream.Close();
                }

                Debug.Log(fileName + "�ϴ��ɹ�");
            }
            catch (Exception ex)
            {
                Debug.Log(fileName + "�ϴ�ʧ��" + ex.Message);
            }
        });

    }
}
