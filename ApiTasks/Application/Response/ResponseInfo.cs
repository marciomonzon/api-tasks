namespace Application.Response
{
    public record ResponseInfo
    {
        public string? Title { get; set; }
        public string? ErrorDescription { get; set; }
        public int HttpStatus { get; set; }
    }
}
