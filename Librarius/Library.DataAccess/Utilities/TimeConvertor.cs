using Library.DataAccess.DTOs;

namespace Library.DataAccess.Utilities;

public static class TimeConvertor
{
    public static int ConvertResponseMinutes(ReadingTimeResponseDto response)
    {
        return (response.Hours * 60) + response.Minutes;
    }
}