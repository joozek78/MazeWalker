using System;
using System.Collections.Generic;
using MazeWalker.Core.Domain;

namespace MazeWalker.Core.FundaApi
{
    public class PropertiesPage
    {
        private PropertiesPage()
        {
            Properties = ArraySegment<Property>.Empty;
            TotalCount = 0;
        }
        
        public PropertiesPage(IReadOnlyCollection<Property> properties, int totalCount)
        {
            Properties = properties;
            TotalCount = totalCount;
        }

        public static PropertiesPage Empty = new PropertiesPage();

        public IReadOnlyCollection<Property> Properties { get; }
        public int TotalCount { get; }
    }
}