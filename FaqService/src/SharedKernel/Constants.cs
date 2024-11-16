namespace SharedKernel;

public class Constants
{
    public const int MAX_TEXT_LENGTH = 5000;
    public const int LOW_TEXT_LENGTH = 300;

    public const string patternRepLink =
        @"^(https:\/\/|http:\/\/)?(www\.)?github\.com\/[a-zA-Z0-9_-]+\/[a-zA-Z0-9_.-]+\/?$";
}