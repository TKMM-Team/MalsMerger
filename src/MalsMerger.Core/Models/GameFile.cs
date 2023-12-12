﻿using System.Text.RegularExpressions;

namespace MalsMerger.Core.Models;

public partial class GameFile
{
    /// <summary>
    /// The file name before the version.
    /// </summary>
    private readonly string? _preName;

    /// <summary>
    /// The file name after the version, or the full file name if no version is in the file name.
    /// </summary>
    private readonly string _name;

    /// <summary>
    /// The root folder (romfs) the file belongs too.
    /// </summary>
    private string _romfs;

    /// <summary>
    /// The game version if it's found in the file name.
    /// </summary>
    public int? Version { get; set; }

    /// <summary>
    /// The prefix name of the <see cref="GameFile"/>.
    /// </summary>
    public string? PreName => _preName;

    /// <summary>
    /// The name of the file with the default version.
    /// </summary>
    public string Name => $"{_preName}.{Version}.{_name}";

    /// <summary>
    /// The folder path relative to <see cref="_romfs"/>
    /// </summary>
    public string Folder { get; }

    public GameFile(string file, string romfs)
    {
        _name = Path.GetFileName(file);
        _romfs = romfs;

        Folder = string.IsNullOrEmpty(romfs) ? string.Empty
            : Path.GetDirectoryName(Path.GetRelativePath(romfs, file)) ?? string.Empty;

        string[] trackedFileParts = VersionPattern().Split(_name);
        if (trackedFileParts.Length == 3 && int.TryParse(trackedFileParts[1], out int version)) {
            Version = version;
            _preName = trackedFileParts[0];
            _name = trackedFileParts[2];
            return;
        }
    }

    public GameFile(string name, string extension, string folder)
    {
        _preName = name;
        _name = extension;
        _romfs = string.Empty;

        Folder = folder;
        Version = TotkConfig.Shared.Version;
    }

    public string? GetVanilla()
    {
        return GetVanilla(Version);
    }

    public string? GetVanilla(int? targetVersion)
    {
        return GetBestMatch(targetVersion, TotkConfig.Shared.GamePath);
    }

    public string? GetBestMatch(int? targetVersion)
    {
        return GetBestMatch(targetVersion, _romfs);
    }

    private string? GetBestMatch(int? targetVersion, string romfs)
    {
        Version = targetVersion;
        string defaultFile = Path.Combine(romfs, Folder, Name);

        if (Version is null || File.Exists(defaultFile)) {
            return defaultFile;
        }

        string folder = Path.Combine(romfs, Folder);

        IEnumerable<GameFile> matches = Directory
            .EnumerateFiles(folder, $"{_preName}*{_name}")
            .Select(x => new GameFile(x, string.Empty))
            .OrderBy(x => x.Version);

        GameFile? match = Directory
            .EnumerateFiles(folder, $"{_preName}*{_name}")
            .Select(x => new GameFile(x, string.Empty))
            .OrderBy(x => x.Version)
            .LastOrDefault();

        Version = match?.Version;
        return match is not null
            ? Path.Combine(folder, match.Name) : null;
    }

    [GeneratedRegex("\\.([0-9]+)\\.")]
    private static partial Regex VersionPattern();
}