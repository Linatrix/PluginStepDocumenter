using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PluginStepDocumenter.Library
{
    static internal class IOrganizationServiceExtensions
    {
        public static List<T> RetrieveAllWithQuery<T>(this IOrganizationService service, QueryExpression query)
            where T : Entity
        {
            // Top count cannot be specified with paging
            if (query.TopCount == null)
            {
                query.PageInfo = new PagingInfo();
                query.PageInfo.Count = 5000;
                query.PageInfo.PageNumber = 1;
                query.PageInfo.PagingCookie = null;
            }

            var entities = new List<T>();

            while (true)
            {
                EntityCollection entityCollection = service.RetrieveMultiple(query);

                if (entityCollection?.Entities?.Count < 1) return entities;

                entities.AddRange(entityCollection.Entities.Select(e => e.ToEntity<T>()).ToList());

                // There are more records so get the next page
                if (entityCollection.MoreRecords)
                {
                    query.PageInfo.PageNumber++;
                    query.PageInfo.PagingCookie = entityCollection.PagingCookie;

                    continue;
                }

                // There are no more records to retrieve, so return the results
                return entities;
            }
        }
    }
}
