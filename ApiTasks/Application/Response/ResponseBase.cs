namespace Application.Response
{
    public record ResponseBase<T>
    {
        public ResponseInfo? ResponseInfo { get; set; }
        public T Value { get; set; }
    }
}
