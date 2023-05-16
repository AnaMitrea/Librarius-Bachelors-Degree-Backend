﻿namespace Library.Application.Models.Reviews;

public class ReviewsRequestModel
{
    public int BookId { get; set; }
    
    public int MaxResults { get; set; }
    
    public string SortBy { get; set; }
    
    public int StartIndex { get; set; }
}