namespace Projctl.Core
{
    #region Namespace Imports

    using Newtonsoft.Json;

    #endregion


    public class SolutionFilterFile
    {
        [JsonProperty("solution")]
        public SolutionFilter SolutionFilter { get; set; }
    }
}