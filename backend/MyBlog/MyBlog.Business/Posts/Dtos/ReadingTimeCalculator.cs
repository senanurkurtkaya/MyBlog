namespace MyBlog.Business.Posts
{
    public static class ReadingTimeCalculator
    {
        public static int Calculate(string? contentMarkdown)
        {
            if (string.IsNullOrWhiteSpace(contentMarkdown))
            {
                return 0;
            }

            var wordCount = contentMarkdown.Split(
                (char[]?)null,
                StringSplitOptions.RemoveEmptyEntries
            ).Length;

            return (int)Math.Ceiling(wordCount / 200.0);
        }
    }
}