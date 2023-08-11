using System.Collections.Generic;
using System.IO;
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

        Object readData = Resources.Load<Object>($"CSV/{GameData.ACCOUNTCSVFILE}");

        if (!readData)
            return answer;
        TextAsset data = readData as TextAsset;

        // �ؽ�Ʈ�������� ��ȯ�� ��� ���� ������
        string[] texts = data.text.Split("\n");             // �����͸� �ٹٲ� ������ ������ ���ڿ� �迭

        for (int i = 0; i < texts.Length; i++)              // �� ���ڿ� ��ҿ� ���Ͽ�
        {
            if (texts[i].Length <= 1)                       // ���̰� 1 ���϶�� ��� ����
                break;                                      // (���� ��� ������ ��� �����Ϳ� �� ���ڿ� �� ���� �߰��Ǳ� ����)
            string[] line = texts[i].Split(delimiter);      // �������� ������ ���ڿ�����

            UserData userData = new UserData();
            userData.id = line[0];
            userData.coin = int.Parse(line[1]);
            userData.avaters = new Dictionary<string, bool>();

            for (int j = 2; j < line.Length - 1; j += 2)
                userData.avaters.Add(line[j], line[j+1] == "True");
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
        Stream fileStream = new FileStream($"Assets/Resources/CSV/{GameData.ACCOUNTCSVFILE}.csv", FileMode.Create, FileAccess.Write);
        // ������ �ּ�, ������ ���ų� ���� ����
        StreamWriter outStream = new(fileStream, Encoding.UTF8);// ��� ����
        outStream.WriteLine(sb);                                // ��Ʈ�������� ����
        outStream.Close();                                      // ��� ����
    }
}