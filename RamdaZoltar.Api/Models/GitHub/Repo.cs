namespace RamdaZoltar.Api.Models.GitHub
{
    public class Repo
    {
        public int Id { get; set; }
        public string Name { get; set; }

        private string _pullsUrl;
        public string PullsUrl
        {
            get
            {
                if (_pullsUrl == null)
                {
                    return _pullsUrl;
                }
                
                var firstBraceIndex = _pullsUrl.IndexOf('{');

                return _pullsUrl.Substring(0, firstBraceIndex);
            }
            set => _pullsUrl = value;
        }
    }
}