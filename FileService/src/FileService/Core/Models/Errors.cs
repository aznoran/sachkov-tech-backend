﻿namespace FileService.Core.Models
{
    public static class Errors
    {
        public static class Files
        {
            public static Error FailUpload() =>
                Error.Failure("file.upload", "Fail to upload file in minio");

            public static Error FailRemove() =>
                Error.Failure("file.remove", "Fail to remove file from minio");
        }
    }
}