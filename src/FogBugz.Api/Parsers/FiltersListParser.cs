using System;
using System.Linq;
using System.Xml.Linq;
using FogBugz.Api.Domain;

namespace FogBugz.Api.Parsers
{
    /// <summary>
    /// Parse response from FogBugz API's cmd=listFilters command.
    /// </summary>
    public class FiltersListParser
    {
        public void Parse(XElement response)
        {
            var filterEnum = response.Element("filters").Elements("filter");
            var filterElements = filterEnum as XElement[] ?? filterEnum.ToArray();

            Filters = filterElements
                .Select(x =>
                    new Filter
                        {
                            Id = (FilterId)x.Attribute("sFilter").Value,
                            Name = x.Value,
                            Type = parseType(x.Attribute("type"))
                        })
                .ToArray();
            var curEle = filterElements
                .FirstOrDefault(e => isCurrentStatus(e.Attribute("status")));
            Current = curEle == null ? null : Filters.First(f => f.Id == curEle.Attribute("sFilter").Value);
        }

        static FilterType parseType(XAttribute type)
        {
            if (type == null)
                throw new NotSupportedException("Unable to parse Filters list. Expected 'type' attribute.");

            var val = type.Value;
            switch (val)
            {
                case "builtin":
                    return FilterType.BuiltIn;
                case "saved":
                    return FilterType.Saved;
                case "shared":
                    return FilterType.Shared;
                default:
                    throw new NotSupportedException(string.Format("Unknown FilterType: '{0}'", val));
            }
        }

        static bool isCurrentStatus(XAttribute status)
        {
            return status != null && status.Value == "current";
        }

        public Filter[] Filters { get; private set; }
        public Filter Current { get; private set; }
    }
}
