﻿namespace Library.Application.DTOs.Trophy;

public class ReadingTimeRequest
{
    public int MinutesReadCounter { get; set; }
    
    public bool CanCheckWin { get; set; }
}