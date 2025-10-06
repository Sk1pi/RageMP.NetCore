﻿using Microsoft.EntityFrameworkCore;

namespace RageMP.Infrastructure.Interfaces.Specification;

public static class SpecificationEvaluator
{
    public static IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, ISpecification<T> specification) where T : class
    {
        var query = inputQuery;
        
        if (specification.Criteria != null)
            query = query.Where(specification.Criteria);
        
        query = specification.Includes.Aggregate(query, 
            (current, include) => current.Include(include));

        if (specification.OrderBy != null)
        {
            if (specification.OrderByDescending)
                query = query.OrderByDescending(specification.OrderBy);
            else
                query = query.OrderBy(specification.OrderBy);
        }
        
        if (specification.IsPagingEnabled)
            query = query.Skip(specification.Skip).Take(specification.Take);

        return query;
    }
}