namespace GarageBuddy.Web.ViewModels
{
    using Newtonsoft.Json;

    public class ModalFormResult
    {
        public ModalFormResult()
        {
            IsValid = false;
            Html = string.Empty;
            RedirectUrl = string.Empty;
        }

        public ModalFormResult(bool isValid, string html, string redirectUrl = "")
        {
            this.IsValid = isValid;
            this.Html = html;
            this.RedirectUrl = redirectUrl;
        }

        [JsonProperty("isValid")]
        public bool IsValid { get; set; }

        [JsonProperty("html")]
        public string Html { get; set; }

        [JsonProperty("redirectUrl")]
        public string? RedirectUrl { get; set; }
    }
}
