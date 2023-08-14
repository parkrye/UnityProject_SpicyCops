using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text;
using UnityEngine;

/// <summary>
/// Ư�� CSW ������ �а� ���� ��ũ��Ʈ
/// </summary>
public static class CSV_RW
{
    static readonly string delimiter = ",";                 // ������

    /// <summary>
    /// ������ �д� �޼ҵ�
    /// </summary>
    /// <param name="fileName">���� �̸�</param>
    /// <returns>��ųʸ� ������ ��� ����</returns>
    public static Dictionary<string, UserData> ReadAccountsCSV()
    {
        Dictionary<string, UserData> answer = new();       // ������ ��ųʸ�

        StreamReader reader;
        try
        {
            reader = new(Application.persistentDataPath + $"/{GameData.ACCOUNTCSVFILE}.csv");
        }
        catch
        {
            Stream fileStream = new FileStream(Application.persistentDataPath + $"/{GameData.ACCOUNTCSVFILE}.csv", FileMode.Create, FileAccess.Write);
            StreamWriter outStream = new StreamWriter(fileStream, Encoding.UTF8);
            outStream.WriteLine();
            return answer;
        }

        bool endOFLine = false;

        while (!endOFLine)
        {
            string line = reader.ReadLine();
            if (line.Length <= 1)
            {
                endOFLine = true;
                break;
            }
            string[] words = line.Split(delimiter);      // �������� ������ ���ڿ�����

            UserData userData = new UserData();
            userData.id = words[0];
            userData.coin = int.Parse(words[1]);
            userData.avaters = new Dictionary<string, bool>();

            for (int j = 2; j < words.Length - 1; j += 2)
                userData.avaters.Add(words[j], words[j + 1] == "True");
            answer.Add(userData.id, userData);
        }

        return answer;                                      // ������ ��ųʸ��� ��ȯ
    }

    /// <summary>
    /// ������ �����ϴ� �޼ҵ�
    /// </summary>
    public static void WriteAccountsCSV()
    {
        StringBuilder sb = new();                               // ������ ��Ʈ������
        foreach (KeyValuePair<string, UserData> pair in GameData.accounts)
        {                                                       // �� ������ �ֿ� ���Ͽ�
            sb.Append(pair.Key);
            sb.Append(delimiter);
            sb.Append(pair.Value.coin);
            sb.Append(delimiter);
            foreach(KeyValuePair<string, bool> pair2 in pair.Value.avaters)
            {
                sb.Append(pair2.Key);
                sb.Append(delimiter);
                sb.Append(pair2.Value);
                sb.Append(delimiter);
            }
            sb.AppendLine();                                    // �ٹٲ��� �����ϱ⸦ �ݺ�
        }
        Stream fileStream = new FileStream(Application.persistentDataPath + $"/{GameData.ACCOUNTCSVFILE}.csv", FileMode.Create, FileAccess.Write);
        // ������ �ּ�, ������ ���ų� ���� ����
        StreamWriter outStream = new(fileStream, Encoding.UTF8);// ��� ����
        outStream.WriteLine(sb);                                // ��Ʈ�������� ����
        outStream.Close();                                      // ��� ����
    }
}