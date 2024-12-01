namespace OrderManagement.Core.Dto
{
    public record OrderStatisticsThresholdReachedDto
    {
        public int OrderStatisticsCount { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
