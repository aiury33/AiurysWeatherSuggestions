namespace AiurysWeatherSuggestions.Models
{
    public class GeminiResponse
    {
        public Candidate[] Candidates { get; set; }
    }

    public class Candidate
    {
        public ModelContent Content { get; set; }
    }

    public class ModelContent
    {
        public Part[] Parts { get; set; }
    }

    public class Part
    {
        public string Text { get; set; }
    }
}
