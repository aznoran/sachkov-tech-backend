namespace SachkovTech.Framework.Authorization;

/// <summary>
/// Доступные пермиссии, делятся по контекстам.
/// </summary>
public static class Permissions
{
    public static class Lessons
    {
        public const string ReadLesson = "lessons.read";
        public const string CreateLesson = "lessons.create";
        public const string UpdateLesson = "lessons.update";
        public const string DeleteLesson = "lessons.delete";
    }

    public static class Modules
    {
        //public const string ReadModule = "modules.read";
        public const string CreateModule = "modules.create";
        public const string UpdateModule = "modules.update";
        public const string DeleteModule = "modules.delete";
    }

    public static class Issues
    {
        public const string ReadIssue = "issues.read";
        public const string CreateIssue = "issues.create";
        public const string UpdateIssue = "issues.update";
        public const string DeleteIssue = "issues.delete";
    }

    public static class SolvingIssues
    {
        //public const string ReadSolvingIssue = "solving.issues.read";
        public const string CreateSolvingIssue = "solving.issues.create";
        public const string UpdateSolvingIssue = "solving.issues.update";
    }

    public static class IssuesReview
    {
        public const string ReadReviewIssue = "review.issues.read";
        public const string CreateReviewIssue = "review.issues.create";
        public const string UpdateReviewIssue = "review.issues.update";
        public const string CommentReviewIssue = "review.issues.comment";
    }

    public static class Files
    {
        //public const string ReadFile = "files.read";
        public const string Upload = "files.create";
    }

    public static class Accounts
    {
        public const string EnrollAccount = "accounts.enroll";
    }
}
