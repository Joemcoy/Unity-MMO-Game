using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Data.Abstracts;

namespace Game.Data.Information
{
    public class VersionInfo : APacketWrapper, IEquatable<VersionInfo>
    {
        public uint Major { get; set; }
        public uint Minor { get; set; }
        public uint Revision { get; set; }
        public uint Build { get; set; }
        public string Name { get; set; }

        public VersionInfo() { }
        public VersionInfo(uint Major, uint Minor, uint Revision, uint Build, string Name)
        {
            this.Major = Major;
            this.Minor = Minor;
            this.Revision = Revision;
            this.Build = Build;
            this.Name = Name;
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}.{3} - {4}", Major, Minor, Revision, Build, Name);
        }

        public bool Equals(VersionInfo Version)
        {
            return
                Version.Major == Major &&
                Version.Minor == Minor &&
                Version.Revision == Revision &&
                Version.Build == Build &&
                Version.Name == Name;
        }
    }
}