namespace GarageBuddy.Web.ViewModels
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);

        public string Title { get; set; } = null!;

        public string Message { get; set; } = null!;

        public int? StatusCode { get; set; }

        public string? ImageUrl { get; set; }
    }
}
