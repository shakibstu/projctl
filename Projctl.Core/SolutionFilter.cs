namespace Projctl.Core
{
    #region Namespace Imports

    using Newtonsoft.Json;

    #endregion


    public class SolutionFilter
    {
        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("projects")]
        public string[] Projects { get; set; }
    }
}