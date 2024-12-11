using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using SharedModels;



namespace API
{
    public class FCMService
    {
        private readonly FirebaseApp _firebaseApp;

        public FCMService()
        {
            _firebaseApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("firebase-adminsdk.json")
            });
        }

        public async Task<string> SendNotificationAsync(NotificationRequestModel notificationRequest)
        {
            var message = new Message()
            {
                Token = notificationRequest.DeviceToken,
                Notification = new Notification()
                {
                    Title = notificationRequest.Title,                 
                }
            };

            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return response;
        }
    }
}
