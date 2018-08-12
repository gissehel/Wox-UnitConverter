using System.Linq;

namespace Wox.EasyHelper.DomainModel
{
    public class WoxQuery
    {
        public object InternalQuery { get; set; }

        public string RawQuery { get; set; }

        public string Search { get; set; }

        public string[] SearchTerms { get; set; }

        public string Command { get; set; }

        public string FirstTerm => SearchTerms.Length > 0 ? SearchTerms[0] : string.Empty;

        public string GetTermOrEmpty(int index) => (SearchTerms.Length > index) ? SearchTerms[index] : string.Empty;

        public string GetAllSearchTermsStarting(int index) => (SearchTerms.Length > index) ? string.Join(" ", SearchTerms.Skip(index).ToArray()) : null;
    }
}