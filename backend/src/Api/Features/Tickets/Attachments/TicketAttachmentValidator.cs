using FluentValidation;
using FluentValidation.Results;
using System.Text;

namespace Api.Features.Tickets.Attachments;

public static class TicketAttachmentValidator
{
    public const long MaximumFileSize = 10 * 1024 * 1024;
    public const int MaximumAttachmentsPerTicket = 5;

    private static readonly IReadOnlyDictionary<string, string[]> AllowedTypes =
        new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            [".pdf"] = ["application/pdf"],
            [".png"] = ["image/png"],
            [".jpg"] = ["image/jpeg"],
            [".jpeg"] = ["image/jpeg"],
            [".webp"] = ["image/webp"],
            [".txt"] = ["text/plain"],
            [".doc"] = ["application/msword"],
            [".docx"] =
            [
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
            ],
            [".xls"] = ["application/vnd.ms-excel"],
            [".xlsx"] =
            [
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            ],
            [".csv"] = ["text/csv", "application/vnd.ms-excel"]
        };

    public static string GetSafeFileName(string fileName)
    {
        var safeFileName = Path.GetFileName(
            fileName.Replace('\\', '/'));

        if (string.IsNullOrWhiteSpace(safeFileName) ||
            safeFileName.Length > 255)
        {
            throw InvalidFile("The filename is invalid.");
        }

        return safeFileName;
    }

    public static async Task<string> ValidateAsync(
        string fileName,
        string contentType,
        long fileSize,
        Stream content,
        CancellationToken cancellationToken)
    {
        var safeFileName = GetSafeFileName(fileName);

        if (fileSize <= 0)
        {
            throw InvalidFile("Empty files cannot be uploaded.");
        }

        if (fileSize > MaximumFileSize)
        {
            throw InvalidFile("The file exceeds the 10 MB limit.");
        }

        var extension = Path.GetExtension(safeFileName);
        if (!AllowedTypes.TryGetValue(extension, out var allowedContentTypes))
        {
            throw InvalidFile("This file extension is not allowed.");
        }

        var providedContentType = contentType
            .Split(';', 2)[0]
            .Trim();

        if (!allowedContentTypes.Contains(
                providedContentType,
                StringComparer.OrdinalIgnoreCase))
        {
            throw InvalidFile("The file MIME type does not match its extension.");
        }

        if (!content.CanSeek)
        {
            throw InvalidFile("The uploaded file could not be validated.");
        }

        var originalPosition = content.Position;
        var prefix = new byte[12];
        var bytesRead = 0;

        try
        {
            while (bytesRead < prefix.Length)
            {
                var read = await content.ReadAsync(
                    prefix.AsMemory(bytesRead),
                    cancellationToken);

                if (read == 0)
                {
                    break;
                }

                bytesRead += read;
            }

            if (!HasValidSignature(extension, prefix, bytesRead))
            {
                throw InvalidFile(
                    "The file content does not match its extension.");
            }
        }
        finally
        {
            content.Position = originalPosition;
        }

        return allowedContentTypes[0];
    }

    public static async Task<IReadOnlyList<ValidatedAttachmentFile>> ValidateFilesAsync(
        IReadOnlyCollection<IFormFile>? files,
        CancellationToken cancellationToken)
    {
        if (files is null || files.Count == 0)
        {
            return [];
        }

        if (files.Count > MaximumAttachmentsPerTicket)
        {
            throw new ValidationException(
            [
                new ValidationFailure(
                    "Attachments",
                    $"A ticket can have at most {MaximumAttachmentsPerTicket} attachments.")
            ]);
        }

        var validatedFiles = new List<ValidatedAttachmentFile>(files.Count);

        foreach (var file in files)
        {
            var safeFileName = GetSafeFileName(file.FileName);
            await using var content = file.OpenReadStream();
            var contentType = await ValidateAsync(
                safeFileName,
                file.ContentType,
                file.Length,
                content,
                cancellationToken);

            validatedFiles.Add(
                new ValidatedAttachmentFile(file, safeFileName, contentType));
        }

        return validatedFiles;
    }

    private static bool HasValidSignature(
        string extension,
        byte[] prefix,
        int length)
    {
        return extension.ToLowerInvariant() switch
        {
            ".pdf" =>
                length >= 5 &&
                prefix.AsSpan(0, 5).SequenceEqual("%PDF-"u8),
            ".png" =>
                length >= 8 &&
                prefix.AsSpan(0, 8).SequenceEqual(
                    new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }),
            ".jpg" or ".jpeg" =>
                length >= 3 &&
                prefix[0] == 0xFF &&
                prefix[1] == 0xD8 &&
                prefix[2] == 0xFF,
            ".webp" =>
                length >= 12 &&
                prefix.AsSpan(0, 4).SequenceEqual("RIFF"u8) &&
                prefix.AsSpan(8, 4).SequenceEqual("WEBP"u8),
            ".doc" or ".xls" =>
                length >= 8 &&
                prefix.AsSpan(0, 8).SequenceEqual(
                    new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 }),
            ".docx" or ".xlsx" =>
                length >= 4 &&
                prefix.AsSpan(0, 4).SequenceEqual(
                    new byte[] { 0x50, 0x4B, 0x03, 0x04 }),
            ".txt" or ".csv" =>
                length > 0 &&
                !prefix.AsSpan(0, length).Contains((byte)0) &&
                IsValidUtf8(prefix, length),
            _ => false
        };
    }

    public sealed record ValidatedAttachmentFile(
        IFormFile File,
        string FileName,
        string ContentType);

    private static bool IsValidUtf8(byte[] prefix, int length)
    {
        try
        {
            _ = new UTF8Encoding(
                encoderShouldEmitUTF8Identifier: false,
                throwOnInvalidBytes: true)
                .GetString(prefix, 0, length);

            return true;
        }
        catch (DecoderFallbackException)
        {
            return false;
        }
    }

    private static ValidationException InvalidFile(string message)
    {
        return new ValidationException(
        [
            new ValidationFailure("File", message)
        ]);
    }
}
