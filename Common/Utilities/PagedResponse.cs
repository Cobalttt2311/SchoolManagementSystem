using SchoolManagementSystem.Common.Responses;

namespace SchoolManagementSystem.Common.Utilities;

public class PagedResponse<T> : ApiResponse<List<T>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }

    public PagedResponse(List<T> data, int pageNumber, int pageSize, int totalRecords)
        : base(true, "Data fetched successfully.", data)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
    }
}