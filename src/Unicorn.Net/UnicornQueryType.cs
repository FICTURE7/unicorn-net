using Unicorn.Internal;

namespace Unicorn
{
    /// <summary>
    /// Types of unicorn-engine query.
    /// </summary>
    public enum UnicornQueryType
    {
        /// <summary>
        /// Queries the mode.
        /// </summary>
        Mode = uc_query_type.UC_QUERY_MODE,

        /// <summary>
        /// Queries the page size.
        /// </summary>
        PageSize = uc_query_type.UC_QUERY_PAGE_SIZE
    }
}
