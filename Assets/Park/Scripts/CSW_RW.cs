using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text;
using UnityEngine;

/// <summary>
/// 특정 CSW 파일을 읽고 쓰는 스크립트
/// </summary>
public static class CSV_RW
{
    static readonly string delimiter = ",";                 // 구분자

    /// <summary>
    /// 파일을 읽는 메소드
    /// </summary>
    /// <param name="fileName">파일 이름</param>
    /// <returns>딕셔너리 구조의 기록 파일</returns>
    public static Dictionary<string, UserData> ReadAccountsCSV()
    {
        Dictionary<string, UserData> answer = new();       // 저장할 딕셔너리

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
            string[] words = line.Split(delimiter);      // 반점으로 분할한 문자열들을

            UserData userData = new UserData();
            userData.id = words[0];
            userData.coin = int.Parse(words[1]);
            userData.avaters = new Dictionary<string, bool>();

            for (int j = 2; j < words.Length - 1; j += 2)
                userData.avaters.Add(words[j], words[j + 1] == "True");
            answer.Add(userData.id, userData);
        }

        return answer;                                      // 저장한 딕셔너리를 반환
    }

    /// <summary>
    /// 파일을 저장하는 메소드
    /// </summary>
    public static void WriteAccountsCSV()
    {
        StringBuilder sb = new();                               // 저장할 스트링빌더
        foreach (KeyValuePair<string, UserData> pair in GameData.accounts)
        {                                                       // 각 데이터 쌍에 대하여
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
            sb.AppendLine();                                    // 줄바꿈을 저장하기를 반복
        }
        Stream fileStream = new FileStream(Application.persistentDataPath + $"/{GameData.ACCOUNTCSVFILE}.csv", FileMode.Create, FileAccess.Write);
        // 저장할 주소, 파일은 쓰거나 새로 생성
        StreamWriter outStream = new(fileStream, Encoding.UTF8);// 출력 형식
        outStream.WriteLine(sb);                                // 스트링빌더를 쓰고
        outStream.Close();                                      // 출력 종료
    }
}