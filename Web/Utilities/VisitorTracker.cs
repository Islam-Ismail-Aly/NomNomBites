namespace Web.Utilities
{
    public class VisitorTracker
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VisitorTracker(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void TrackUniqueVisitor()
        {
            string sessionId = _httpContextAccessor.HttpContext.Session.Id;

            if (string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Session.GetString("Visited")))
            {
                _httpContextAccessor.HttpContext.Session.SetString("Visited", "true");

                if (_httpContextAccessor.HttpContext.Session.GetInt32("UniqueVisitorCount") == null)
                {
                    _httpContextAccessor.HttpContext.Session.SetInt32("UniqueVisitorCount", 1);
                }
                else
                {
                    int count = _httpContextAccessor.HttpContext.Session.GetInt32("UniqueVisitorCount").Value;
                    count++;
                    _httpContextAccessor.HttpContext.Session.SetInt32("UniqueVisitorCount", count);
                }
            }
        }

        public int GetUniqueVisitorCount()
        {
            return _httpContextAccessor.HttpContext.Session.GetInt32("UniqueVisitorCount") ?? 0;
        }
    }
}
