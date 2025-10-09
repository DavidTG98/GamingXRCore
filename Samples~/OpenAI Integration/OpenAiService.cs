using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace GamingXRCore.OpenAIIntegration
{
    public static class OpenAiService
    {
        private const string CHAT_URL = "https://api.openai.com/v1/chat/completions";

        private static List<AIChatMessage> currentMessages = new List<AIChatMessage>();
        private static List<UnityWebRequest> requestsInProgress = new List<UnityWebRequest>();

        public static void SendMessageToChat(MonoBehaviour monoBehaviour, SO_OpenAiModel aiModel, string message, Action<string> OnSuccess = null, Action<string> OnError = null)
        {
            UpdateSystemMessage(aiModel);

            AIChatMessage chatMessage = new()
            {
                role = "user",
                content = message
            };

            currentMessages.Add(chatMessage);
            ChatRequest chatRequest = new()
            {
                model = aiModel.gPTModel.ToString().Replace('_', '-'),
                messages = currentMessages,
                temperature = aiModel.chatTemperature,
            };

            string jsonRequest = JsonUtility.ToJson(chatRequest);

            var request = UnityWebRequest.Post(CHAT_URL, jsonRequest, "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + aiModel.openAiApiKey);
            request.SetRequestHeader("OpenAI-Organization", aiModel.openAiOrganizationId);

            monoBehaviour.StartCoroutine(SendRequest(request, OnSuccess, OnError));
        }

        static IEnumerator SendRequest(UnityWebRequest request, Action<string> OnSuccess = null, Action<string> OnError = null)
        {
            requestsInProgress.Add(request);
            yield return request.SendWebRequest();
            requestsInProgress.Remove(request);

            if (request.result != UnityWebRequest.Result.Success)
            {
                OnError?.Invoke(request.downloadHandler.error);
                Debug.LogError("Request error: " + request.downloadHandler.error);
                yield break;
            }

            AIChatMessage responseMessage = null;
            try
            {
                responseMessage = JsonUtility.FromJson<ChatResponse>(request.downloadHandler.text).choices[0].message;
            }
            catch
            {
                OnError?.Invoke("There was a error deserializing the json");
                Debug.LogError("INVALID JSON RESPONSE");
                Debug.LogError(request.downloadHandler.text);
                yield break;
            }

            currentMessages.Add(responseMessage);

            OnSuccess?.Invoke(responseMessage.content);
            request.Dispose();
        }

        public static void AbortRequests()
        {
            foreach (var req in requestsInProgress)
                req.Abort();
        }

        private static void UpdateSystemMessage(SO_OpenAiModel aiModel)
        {
            if (currentMessages.Count > 0)
            {
                currentMessages[0].content = aiModel.chatContext + DateUtil.SystemDateInfo();
            }
            else
            {
                currentMessages.Add(new AIChatMessage()
                {
                    role = "system",
                    content = aiModel.chatContext + DateUtil.SystemDateInfo(),
                });
            }
        }

        // CHAT OBJECTS
        [Serializable]
        private struct ChatRequest
        {
            public string model;
            public List<AIChatMessage> messages;
            public float temperature;
        }

        [Serializable]
        private class AIChatMessage
        {
            public string role;
            [TextArea(1, 8)] public string content;
        }

        [Serializable]
        private class ChatResponse
        {
            public string id;
            public List<ChatChoice> choices;
        }

        [Serializable]
        private class ChatChoice
        {
            public int index;
            public AIChatMessage message;
            public string finish_reason;
        }
    }
}