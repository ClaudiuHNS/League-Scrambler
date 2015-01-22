﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace League.Files
{
    public class Archive
    {
        internal const uint MAGIC_VALUE = 0x18be0ef0;

        public string FilePath { get { return _filePath; } internal set { _filePath = value; DataFilePath = value + ".dat"; } }
        public string DataFilePath { get; private set; }
        public uint ArchiveIndex { get; internal set; }
        public uint ArchiveVersion { get; internal set; }
        public Dictionary<string, ArchiveFileInfo> Files { get; internal set; }

        private string _filePath;

        public void SaveChanges()
        {
            var writer = new ArchiveWriter();
            writer.WriteArchive(this, FilePath);
        }

        public string GetRelativeFilePath(string leaguePath)
        {
            return FilePath.Replace(leaguePath, "");
        }

        public string GetRelateiveDataFilePath(string leaguePath)
        {
            return DataFilePath.Replace(leaguePath, "");
        }

        public uint GetManagerIndex()
        {
            var dirname = Path.GetDirectoryName(FilePath).Split('\\').Last();
            var values = dirname.Split('.');
            var result = BitConverter.ToUInt32(new byte[] { Convert.ToByte(values[3]), Convert.ToByte(values[2]), Convert.ToByte(values[1]), Convert.ToByte(values[0]) }, 0);
            return result;
        }

        public static Archive LoadFromFile(string filepath)
        {
            var reader = new ArchiveReader();
            return reader.ReadArchive(filepath);
        }
    }

    public class ArchiveFileInfo
    {
        public string Path { get; set; }
        public uint DataOffset { get; set; }
        public uint DataLength { get; set; }
    }
}
