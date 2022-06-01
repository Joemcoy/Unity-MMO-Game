using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Xml;
using System.Diagnostics;

using Base.Data.Interfaces;
using System.Reflection;
using Base.Helpers;
using Base.Factories;

namespace Base.Data.Abstracts
{
	public abstract class XMLConfiguration : IConfiguration, IComponent, ISingleton
	{
		public abstract string Filename { get; }
        public virtual bool Secure { get { return false; } }
		public abstract void WriteDefaults();

		public const BindingFlags Flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy;
        private readonly string UTF8BOMMark = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
        
#if !UNITY_EDITOR &&UNITY_5
        private readonly string Target = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games"), "RPG Project");
#else
        private readonly string Target = Path.Combine(Environment.CurrentDirectory, "config");
#endif

        public virtual bool Load()
        {
            string Filename = Path.Combine(Target, this.Filename);

            if (!File.Exists(Filename))
            {
                WriteDefaults();
                return Save();
            }

            byte[] Data = File.ReadAllBytes(Filename);

            if(Secure)
                Data = RijndaelHelper.Decrypt(Data);

            using (MemoryStream Memory = new MemoryStream(Data))
            {
                using (XmlTextReader Reader = new XmlTextReader(Memory))
                {
                    XmlDocument Document = new XmlDocument();
                    Document.Load(Reader);

                    DeserializeNode(Document.DocumentElement, this, true);
                }
            }
            return true;
        }

        void DeserializeNode(XmlNode Node, object Target, bool Static = false)
        {
            foreach (XmlNode SubNode in Node)
            {
                if (SubNode is XmlComment)
                    continue;

                PropertyInfo Property = Static ? Target.GetType().GetProperty(SubNode.Name, Flags) : Target.GetType().GetProperty(SubNode.Name);
                object Value = Deserialize(Property, SubNode);

                Property.SetValue(Static ? null : Target, Value, null);
            }
        }

        object Deserialize(PropertyInfo Property, XmlNode Node)
        {
            if (Property.PropertyType == typeof(Guid))
                return new Guid(Node.InnerText);
            else if (Property.PropertyType.IsPrimitive || Property.PropertyType.IsEnum || Property.PropertyType == typeof(string))
                return Convert.ChangeType(Node.InnerText, Property.PropertyType);
            else if (Property.PropertyType.IsArray)
            {
                var EType = Property.PropertyType.GetElementType();
                //var A = Array.CreateInstance(EType, Node.ChildNodes.Count);

                /*for (int i = 0; i < A.Length; i++)
                {
                    var SubNode = Node.ChildNodes[i];
                    if (SubNode is XmlComment)
                        continue;

                    if (EType == typeof(string) || EType.IsPrimitive)
                        A.SetValue(Convert.ChangeType(SubNode.InnerText, EType), i);
                    else
                        A.SetValue(DeserializeObject(Property.PropertyType.GetElementType(), SubNode), i);
                }*/

                var Temp = new List<object>();
                foreach(XmlNode SubNode in Node.ChildNodes)
                {
                    if (SubNode is XmlComment)
                        continue;

                    object Value = null;
                    if (EType == typeof(string) || EType.IsPrimitive)
                        Value = Convert.ChangeType(SubNode.InnerText, EType);
                    else
                        Value = DeserializeObject(Property.PropertyType.GetElementType(), SubNode);

                    Temp.Add(Value);
                }

                var A = Array.CreateInstance(EType, Temp.Count);
                Array.Copy(Temp.ToArray(), A, Temp.Count);
                
                return A;
            }
            else
                return DeserializeObject(Property.PropertyType, Node);
        }

        object DeserializeObject(Type TargetType, XmlNode Node)
        {
            var Instance = Activator.CreateInstance(TargetType);
            DeserializeNode(Node, Instance);

            return Instance;
        }

        public virtual bool Save()
        {
            string Filename = Path.Combine(Target, this.Filename);
            if (!Directory.Exists(Path.GetDirectoryName(Filename)))
                Directory.CreateDirectory(Path.GetDirectoryName(Filename));

            if (File.Exists(Filename))
                File.Delete(Filename);

            using (FileStream Stream = File.Open(Filename, FileMode.OpenOrCreate))
            {
                using (XmlTextWriter Writer = new XmlTextWriter(Stream, Encoding.UTF8))
                {
                    Writer.Formatting = Formatting.Indented;

                    XmlDocument Document = new XmlDocument();
                    Document.AppendChild(Document.CreateXmlDeclaration("1.0", "UTF-8", "yes"));

                    XmlNode RootNode = Document.AppendChild(Document.CreateElement("root"));
                    foreach (PropertyInfo Property in GetType().GetProperties(Flags))
                    {
                        if (Property.Name != "Filename")
                        {
                            object Value = Property.GetValue(null, null);

                            Serialize(Property, RootNode, Value);
                        }
                    }
                    Document.Save(Writer);
                }
            }

            if (Secure)
            {
                byte[] Data = File.ReadAllBytes(Filename);
                Data = RijndaelHelper.Encrypt(Data);
                File.WriteAllBytes(Filename, Data);
            }

            return true;
        }

        void Serialize(PropertyInfo Property, XmlNode Node, object Value)
        {
            var PNode = Node.AppendChild(Node.OwnerDocument.CreateElement(Property.Name));

            if (Property.PropertyType == typeof(string) || Property.PropertyType.IsPrimitive)
                PNode.InnerText = Convert.ToString(Value);
            else if (Property.PropertyType.IsEnum)
                PNode.InnerText = Enum.GetName(Property.PropertyType, Value);
            else if (Property.PropertyType.IsArray)
            {
                var Array = (Array)Value;

                for (int i = 0; i < Array.Length; i++)
                {
                    var ANode = PNode.AppendChild(Node.OwnerDocument.CreateElement("ArrayItem"));
                    var AValue = Array.GetValue(i);

                    if (AValue is string || AValue.GetType().IsPrimitive)
                        ANode.InnerText = Convert.ToString(AValue);
                    else
                        SerializeObject(ANode, AValue);
                }
            }
            else
                SerializeObject(PNode, Value);
        }

        void SerializeObject(XmlNode Node, object Value)
        {
            foreach (var OProperty in Value.GetType().GetProperties())
            {
                Serialize(OProperty, Node, OProperty.GetValue(Value, null));
            }
        }

		public bool Enable()
		{
            if(!Load())
            {
                WriteDefaults();
                return Save();
            }
            return true;
		}

		public bool Disable()
		{
			return Save();
		}

		public void Create()
		{
         
		}

		public void Destroy()
		{
         
		}
	}
}