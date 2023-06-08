﻿namespace Trophy.Application.Models.Trophy.Response;

public class TrophyModel
{
    public int Id { get; set; }
    
    public string Title { get; set; }

    public string Instructions { get; set; }
    
    public string ImageSrcPath { get; set; }
    public bool IsWon { get; set; }
}