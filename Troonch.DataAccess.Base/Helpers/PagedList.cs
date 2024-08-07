﻿namespace Troonch.DataAccess.Base.Helpers;

public class PagedList<T> where T : class
{
    private PagedList(List<T> collections, int page, int pageSize, int totalCount,decimal totalPages)
    {
        Collections = collections;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = totalPages;
    }
    public List<T> Collections { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public decimal TotalPages { get; }
    public bool HasNextPage => Page * PageSize < TotalCount;
    public bool HasPreviousPage => Page > 1;

    public static PagedList<T> Create(IEnumerable<T>? list, int page, int pageSize)
    {
        int totalCount = list.Count();
        var collection =  list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        decimal pagesAvailable = Math.Ceiling((decimal)list.Count() / (decimal)pageSize);
        var totalPages =  Math.Ceiling(pagesAvailable);

        return new PagedList<T>(collection, page, pageSize, totalCount, totalPages);
    }
}
