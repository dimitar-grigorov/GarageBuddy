namespace GarageBuddy.Services.Messaging.Email
{
    public class EmailAttachment
    {
        public byte[] Content { get; set; }

        public string FileName { get; set; }

        public string MimeType { get; set; }
    }
}
