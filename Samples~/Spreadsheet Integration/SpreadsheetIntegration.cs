using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace GamingXRCore.SpreadsheetIntegration
{
    public static class SpreadsheetIntegration
    {
        public static void ReadSheet(MonoBehaviour mono, string webAppUrl, Action<List<List<string>>> callback)
        {
            mono.StartCoroutine(ReadSheetCoroutine(webAppUrl, callback));
        }

        public static void WriteSheet(MonoBehaviour mono, string webAppUrl, List<List<string>> values, Action<bool> callback)
        {
            mono.StartCoroutine(WriteSheetCoroutine(webAppUrl, values, callback));
        }

        public static void AppendRow(MonoBehaviour mono, string webAppUrl, List<string> values, Action<bool> callback)
        {
            mono.StartCoroutine(AppendRowCoroutine(webAppUrl, values, callback));
        }

        public static void AppendMultipleRows(MonoBehaviour mono, string webAppUrl, List<List<string>> rows, Action<bool> callback)
        {
            mono.StartCoroutine(AppendMultipleRowsCoroutine(webAppUrl, rows, callback));
        }

        private static IEnumerator ReadSheetCoroutine(string webAppUrl, Action<List<List<string>>> callback)
        {
            string url = $"{webAppUrl}?range=";

            using UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;

                if (jsonResponse.TrimStart().StartsWith("<"))
                {
                    Debug.LogError("Received HTML error page instead of JSON");
                    Debug.LogError("Response: " + jsonResponse.Substring(0, Mathf.Min(500, jsonResponse.Length)));
                    callback?.Invoke(null);
                    yield break;
                }

                try
                {
                    AppsScriptResponse response = JsonConvert.DeserializeObject<AppsScriptResponse>(jsonResponse);

                    if (response.success)
                    {
                        List<List<string>> result = new List<List<string>>();

                        if (response.data != null)
                        {
                            foreach (var row in response.data)
                            {
                                List<string> rowList = new List<string>();
                                foreach (var cell in row)
                                {
                                    rowList.Add(cell);
                                }
                                result.Add(rowList);
                            }
                        }

                        callback?.Invoke(result);
                    }
                    else
                    {
                        Debug.LogError("Read failed: " + response.message);
                        callback?.Invoke(null);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("JSON Parse Error: " + ex.Message);
                    Debug.LogError("Response was: " + jsonResponse);
                    callback?.Invoke(null);
                }
            }
            else
            {
                Debug.LogError($"Network error: {request.error}");
                Debug.LogError($"Response Code: {request.responseCode}");
                callback?.Invoke(null);
            }
        }

        private static IEnumerator WriteSheetCoroutine(string webAppUrl, List<List<string>> values, Action<bool> callback)
        {
            WriteRequest requestData = new WriteRequest
            {
                action = "write",
                range = "",
                values = ConvertToStringArray(values)
            };

            string json = JsonConvert.SerializeObject(requestData);
            Debug.Log("Write Request: " + json);

            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

            using UnityWebRequest request = new UnityWebRequest(webAppUrl, "POST");
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log("Write Response: " + jsonResponse);

                AppsScriptResponse response = JsonConvert.DeserializeObject<AppsScriptResponse>(jsonResponse);
                callback?.Invoke(response.success);
            }
            else
            {
                Debug.LogError($"Write error: {request.error}");
                callback?.Invoke(false);
            }
        }

        private static IEnumerator AppendRowCoroutine(string webAppUrl, List<string> values, Action<bool> callback)
        {
            AppendRequest requestData = new AppendRequest
            {
                action = "append",
                values = values.ToArray()
            };

            string json = JsonConvert.SerializeObject(requestData);
            Debug.Log("Append Request: " + json);

            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

            using (UnityWebRequest request = new UnityWebRequest(webAppUrl, "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string jsonResponse = request.downloadHandler.text;
                    Debug.Log("Append Response: " + jsonResponse);

                    AppsScriptResponse response = JsonConvert.DeserializeObject<AppsScriptResponse>(jsonResponse);
                    callback?.Invoke(response.success);
                }
                else
                {
                    Debug.LogError($"Append error: {request.error}");
                    callback?.Invoke(false);
                }
            }
        }



        private static IEnumerator AppendMultipleRowsCoroutine(string webAppUrl, List<List<string>> rows, Action<bool> callback)
        {
            WriteRequest requestData = new WriteRequest
            {
                action = "appendMultiple",
                range = "",
                values = ConvertToStringArray(rows)
            };

            string json = JsonConvert.SerializeObject(requestData);
            Debug.Log("Append Multiple Request: " + json);

            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

            using (UnityWebRequest request = new UnityWebRequest(webAppUrl, "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string jsonResponse = request.downloadHandler.text;
                    Debug.Log("Append Multiple Response: " + jsonResponse);

                    AppsScriptResponse response = JsonConvert.DeserializeObject<AppsScriptResponse>(jsonResponse);
                    callback?.Invoke(response.success);
                }
                else
                {
                    Debug.LogError($"Append Multiple error: {request.error}");
                    callback?.Invoke(false);
                }
            }
        }

        // Helper method to convert List<List<string>> to string[][]
        private static string[][] ConvertToStringArray(List<List<string>> values)
        {
            string[][] result = new string[values.Count][];
            for (int i = 0; i < values.Count; i++)
            {
                result[i] = values[i].ToArray();
            }
            return result;
        }

        [Serializable]
        private class AppsScriptResponse
        {
            public bool success;
            public string message;
            public string[][] data;
        }

        [Serializable]
        private class WriteRequest
        {
            public string action;
            public string range;
            public string[][] values;
        }

        [Serializable]
        private class AppendRequest
        {
            public string action;
            public string[] values;
        }
    }
}