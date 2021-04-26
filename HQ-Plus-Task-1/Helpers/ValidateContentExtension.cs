using System.Text.RegularExpressions;

namespace HQ_Plus_Task_1.Helpers
{
    public static class ValidateContentExtension
    {
        public static bool ValidateContent(string content)
        {
            Regex tagRegex = new Regex(@"<\s*([^ >]+)[^>]*>.*?<\s*/\s*\1\s*>");
            bool hasTags = tagRegex.IsMatch(content);
            if (!tagRegex.IsMatch(content))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
