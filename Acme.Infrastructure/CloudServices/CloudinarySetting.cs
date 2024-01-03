﻿namespace Acme.Infrastructure.CloudServices;

internal class CloudinarySetting
{
    public const string SectionName = "CloudinarySettings";
    public string CloudName { get; init; } = null!;
    public string ApiKey { get; init; } = null!;
    public string ApiSecret { get; init; } = null!;
}