﻿using CommandLine;
using System;
using System.Reflection;
using Yuniql.Core;
using Yuniql.Extensibility;

namespace Yuniql.CLI
{
    public class Program
    {
        //https://github.com/commandlineparser/commandline
        //https://github.com/dotnet/command-line-api

        public static int Main(string[] args)
        {
            var traceService = new FileTraceService();
            var directoryService = new DirectoryService();
            var fileService = new FileService();
            var workspaceService = new WorkspaceService(traceService, directoryService, fileService);

            var environmentService = new EnvironmentService();
            var configurationService = new ConfigurationService(environmentService, workspaceService, traceService);

            var migrationServiceFactory = new MigrationServiceFactory(traceService);
            var commandLineService = new CommandLineService(migrationServiceFactory,
                                                            workspaceService,
                                                            environmentService,
                                                            traceService,
                                                            configurationService);

            var resultCode = Parser.Default
                .ParseArguments<
                    PingOption,
                    InitOption,
                    RunOption,
                    ListOption,
                    NextVersionOption,
                    VerifyOption,
                    EraseOption,
                    BaselineOption,
                    RebaseOption,
                    ArchiveOption,
                    PlatformsOption
                >(args).MapResult(
                    (PingOption opts) => Dispatch(commandLineService.RunPingOption, opts, traceService),
                    (InitOption opts) => Dispatch(commandLineService.RunInitOption, opts, traceService),
                    (RunOption opts) => Dispatch(commandLineService.RunRunOption, opts, traceService),
                    (ListOption opts) => Dispatch(commandLineService.RunListOption, opts, traceService),
                    (NextVersionOption opts) => Dispatch(commandLineService.RunNextVersionOption, opts, traceService),
                    (VerifyOption opts) => Dispatch(commandLineService.RunVerifyOption, opts, traceService),
                    (EraseOption opts) => Dispatch(commandLineService.RunEraseOption, opts, traceService),
                    (BaselineOption opts) => Dispatch(commandLineService.RunBaselineOption, opts, traceService),
                    (RebaseOption opts) => Dispatch(commandLineService.RunRebaseOption, opts, traceService),
                    (ArchiveOption opts) => Dispatch(commandLineService.RunArchiveOption, opts, traceService),
                    (PlatformsOption opts) => Dispatch(commandLineService.RunPlatformsOption, opts, traceService),

                    errs => 1);

            return resultCode;
        }

        private static int Dispatch<T>(Func<T, int> command, T opts, ITraceService traceService)
            where T : BaseOption
        {
            var toolVersion = typeof(CommandLineService).Assembly.GetName().Version;
            var toolPlatform = Environment.OSVersion.Platform == PlatformID.Win32NT ? "windows" : "linux";
            var toolCopyright = (typeof(CommandLineService).Assembly
                .GetCustomAttribute(typeof(AssemblyCopyrightAttribute)) as AssemblyCopyrightAttribute).Copyright;

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Running yuniql v{toolVersion.Major}.{toolVersion.Minor}.{toolVersion.Build} for {toolPlatform}-x64");
            Console.WriteLine($"{toolCopyright}. Apache License v2.0");
            Console.WriteLine($"Visit https://yuniql.io for documentation & more samples{Environment.NewLine}");
            Console.ResetColor();

            traceService.IsDebugEnabled = opts.IsDebug;
            traceService.TraceSensitiveData = opts.TraceSensitiveData;

            return command.Invoke(opts);
        }
    }
}
