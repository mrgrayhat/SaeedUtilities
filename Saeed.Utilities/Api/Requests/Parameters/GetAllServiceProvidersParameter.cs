using System.Collections.Generic;

namespace Saeed.Utilities.API.Requests.Parameters
{
    public class GetAllServiceProvidersParameter : RequestParameterWithSort
    {
        /// <summary>
        /// شهر مورد فعالیت
        /// </summary>
        public int CityId { get; set; } = 0;
        /// <summary>
        /// جنسیت
        /// </summary>
        public int Gender { get; set; } = 1;
        /// <summary>
        /// سطح
        /// </summary>
        public int Level { get; set; } = 0;

        public long Experience { get; set; } = 0;
        /// <summary>
        /// تخصص
        /// </summary>
        public int Specialty { get; set; } = 1;
        public List<int> Categories { get; set; }
        public string UserId { get; set; }
        public string SearchTerm { get; set; } = null;
        public bool Studios { get; set; }
    }
}