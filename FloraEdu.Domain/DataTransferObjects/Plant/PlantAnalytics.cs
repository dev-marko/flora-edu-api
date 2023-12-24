using FloraEdu.Domain.DataTransferObjects.Analytics;

namespace FloraEdu.Domain.DataTransferObjects.Plant;

public class PlantAnalytics
{
    public string MostPopularByLikes { get; set; }
    public int MostPopularByLikesCount { get; set; }
    public string MostPopularByBookmarks { get; set; }
    public int MostPopularByBookmarksCount { get; set; }
    public string MostPopularByNumberOfComments { get; set; }
    public int MostPopularByNumberOfCommentsCount { get; set; }
    public string MostPopularByUniqueVisitors { get; set; }
    public int MostPopularByUniqueVisitorsCount { get; set; }
    public List<LikesDataDto> LikesChartData { get; set; } = new();
    public List<BookmarksDataDto> BookmarksChartData { get; set; } = new();
}
