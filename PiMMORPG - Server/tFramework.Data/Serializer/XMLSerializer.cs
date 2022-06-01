using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Xml;
using System.IO;

namespace tFramework.Data.Serializer
{
    using Extensions;
    using Bases;

    public class XMLSerializer : SerializerBase<XMLSerializer>
    {
        public static readonly XmlWriterSettings WriterSettings = new XmlWriterSettings
        {
            Indent = true,
            IndentChars = " ",
            NewLineChars = "\n "
        };

        protected override SerializerElement LoadData<T>(Stream source)
        {
            XmlDocument document = new XmlDocument();
            document.Load(source);

            return LoadNode(document.DocumentElement);
        }

        protected SerializerElement LoadNode(XmlNode node)
        {
            var element = new SerializerElement(node.Name);
            var attributes = node.Attributes.OfType<XmlAttribute>();
            var childrens = node.OfType<XmlNode>();

            element.Attributes = attributes.Select(a => new SerializerAttribute(a.Name, a.Value)).ToList();
            element.Childrens = childrens.Select(c => LoadNode(c)).ToList();
            return element;
        }

        protected override void WriteData(Stream source, SerializerElement element, string comments = null)
        {
            using (var writer = XmlWriter.Create(source, WriterSettings))
            {
                var document = new XmlDocument();
                document.AppendChild(document.CreateXmlDeclaration("1.0", "UTF-8", null));
                if (comments != null)
                    document.AppendChild(document.CreateComment(comments));

                var root = document.AppendChild(document.CreateElement(element.Name));
                foreach (var attribute in element.Attributes)
                    root.Attributes.Append(document.CreateAttribute(attribute.Name)).Value = attribute.Value;

                foreach (var children in element.Childrens)
						AppendElement(root, children);
                document.Save(writer);
            }
        }

        void AppendElement(XmlNode parent, SerializerElement element)
        {
            var document = parent.OwnerDocument;
            var node = parent.AppendChild(document.CreateElement(element.Name));
            /*if (element.Childrens.Length == 0)
				parent.Attributes.Append(document.CreateAttribute(element.Name)).Value = element.Value;
			else
			{
				var node = parent.AppendChild(document.CreateElement(element.Name));
				foreach (var children in element.Childrens)
					AppendElement(node, children);
			}*/

            foreach (var attribute in element.Attributes)
                node.Attributes.Append(document.CreateAttribute(attribute.Name)).Value = attribute.Value;
            foreach (var children in element.Childrens)
                AppendElement(node, children);
        }
    }
}