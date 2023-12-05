﻿using System.Reflection;

namespace MalsMerger;

public static class Const
{
    public static readonly string Help = $"""
        TotK Mals Merger [Version {Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "Undefined"}]
        (c) Arch Leaders. MIT License

        Usage:
          <input_folder(s)> <output_folder> [-l|-log LOG_FILE] [-v|--verbose VERBOSE]

          Input Folders: (Path)
            Bar (|) seperated list of the input mod folders. [Priority: highest to lowest <-> left to right]

              Note: This should be one argument surounded by quotes.
                    Quotation marks should NOT be around each path (see examples)

          Output Folder: (Path)
            The output mod folder to put the merged Mals archives

          Log File: (Path)
            Specify a path to write logs to (logging disabled by default)

          Verbose: (Boolean)
            Enable verbose logging

        Examples:
          "path/to/mod_a|path/to/mod_b" "path/to/mod_final" -l mals-merger.log -v true
        """;
}